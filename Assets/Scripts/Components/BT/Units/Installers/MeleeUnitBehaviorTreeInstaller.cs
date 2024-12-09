using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using BT.Nodes.Actions;
using BT.Nodes.Actions.Animation;
using BT.Nodes.Conditionals;
using BT.Nodes.Events;
using BT.Shared.Containers;
using BT.Tools;
using BT.Utility;
using Combat.CombatTargetsProviders.Interfaces;
using Components.Animation;
using Components.Animation.Interfaces;
using Components.BT.Actions;
using Components.BT.Actions.Interfaces;
using Components.BT.Interfaces;
using Components.BT.Units.Installers.Data;
using Components.Combat.Interfaces;
using Components.Health;
using Components.Movement.Interfaces;
using Sirenix.Utilities;
using Utility;

namespace Components.BT.Units.Installers
{
    public class MeleeUnitBehaviorTreeInstaller : IBehaviorTreeInstaller //used in config
    {
        private BehaviorTree _behaviorTree;
        private Dictionary<Type, SharedVariable> _sharedContainers;
        private BehaviorTreeCachedVariablesHolder _cachedVariablesHolder;

        public void Install(BehaviorTree behaviorTree, IBehaviorTreeInstallerData behaviorTreeInstallerData)
        {
            _behaviorTree = behaviorTree;
            
            if (behaviorTreeInstallerData is not MeleeUnitBehaviorTreeInstallerData installerData)
            {
                throw new Exception("Incorrect installer data!");
            }

            SetupSharedContainers(installerData.EntityHolder, installerData.DamageableComponent);
            
            SetupSelfTasks(installerData.AnimationPlayer, installerData.MovementProvider);
            SetupNavMeshMovementTasks(installerData.MovementProvider);
            SetupCombatTasks(installerData.CombatActions, installerData.CombatTargetsProvider,
                    installerData.WeaponStatsProvider, installerData.MovementProvider, installerData.CombatTargetHolder);
            
            installerData.BehaviorTreeStarter.BehaviorTreeStartEvent += ()=> _behaviorTree.FindTask<InPreparingProcess>().SetReady(); 
        }

        private void SetupSharedContainers(EntityHolder entityHolder, IDamageable damageableComponent)
        {
            _cachedVariablesHolder = new BehaviorTreeCachedVariablesHolder(_behaviorTree);
            _sharedContainers = new Dictionary<Type, SharedVariable>();
            
            SetContainer<MovementSharedContainerVariable>();
            SetContainer<DamageableTargetSharedContainerVariable>();
            
            var selfContainer =SetContainer<SelfGeneralDataSharedContainerVariable>();
            selfContainer.Value.SelfTransform = entityHolder.SelfTransform;
            selfContainer.Value.SelfDamageable = (HealthComponent) damageableComponent;
            
            T SetContainer<T>() where T: SharedVariable
            {
                T container = _cachedVariablesHolder.GetVariable<T>();
                _sharedContainers.Add(container.GetType(), container);
                return container;
            }
        }

        private void SetupSelfTasks(IAnimationPlayer animationPlayer, IMovementProvider movementProvider)
        {
            var selfDataContainer = GetSharedContainer<SelfGeneralDataSharedContainerVariable>().Value;
            
            _behaviorTree
                .FindTask<SelfValidate>()
                .SetSharedVariables(selfDataContainer.SelfDamageable);

            _behaviorTree.FindTasks<SetAnimation>(CommonBehaviorTasksNames.SetIdleAnimation).ForEach((x) =>
            {
                x.Initialize(animationPlayer, new AnimationClipData(targetStateName: AnimationStatesNames.Idle));
            });
            
            _behaviorTree.FindTasks<SetAnimation>(CommonBehaviorTasksNames.SetMovementAnimation).ForEach((x) =>
            {
                x.Initialize(animationPlayer, new AnimationClipData(targetStateName: AnimationStatesNames.Moving));
            });
            
            _behaviorTree.FindTasks<SetDeath>().ForEach(x =>
            {
                x.Initialize(movementProvider,animationPlayer);
            });
        }

        private void SetupNavMeshMovementTasks(IMovementProvider movementProvider)
        {
            var movementSharedContainer = GetSharedContainer<MovementSharedContainerVariable>().Value;
            var damageableTargetSharedContainer = GetSharedContainer<DamageableTargetSharedContainerVariable>().Value;
            var selfContainer = GetSharedContainer<SelfGeneralDataSharedContainerVariable>().Value;

            _behaviorTree
                .FindTask<MoveToPoint>(CommonBehaviorTasksNames.MoveToPoint)
                .Initialize(movementProvider).SetSharedVariables(movementSharedContainer.CurrentDestinationPoint);
            
            _behaviorTree.FindTasks<StopMoving>().ForEach(x=>x.Initialize(movementProvider));
            
            _behaviorTree.FindTask<FindTargetClosestPoint>()
                .SetSharedVariables(damageableTargetSharedContainer.TargetTr,
                    damageableTargetSharedContainer.TargetDamageable,
                    selfContainer.SelfTransform,
                    selfContainer.SelfDamageable,
                    movementSharedContainer.CurrentDestinationPoint);
            
            _behaviorTree
                .FindTask<SetClosestNavMeshPoint>()
                .SetSharedVariables(movementSharedContainer.CurrentDestinationPoint);
        }

        private void SetupCombatTasks(IEnumerable<IBehaviorAction> combatActions, ICombatTargetsProvider combatTargetsProvider,
            IWeaponStatsProvider weaponStatsProvider, IMovementProvider movementProvider, ICombatTargetHolder combatTargetHolder)
        {
            var damageableTargetSharedContainer = GetSharedContainer<DamageableTargetSharedContainerVariable>().Value;
            var selfGeneralContainer = GetSharedContainer<SelfGeneralDataSharedContainerVariable>().Value;

            _behaviorTree
                .FindTasks<ValidateDamageableTarget>()
                .ForEach(x =>
                {
                    x.Initialize(movementProvider)
                        .SetSharedVariables
                            (damageableTargetSharedContainer.TargetDamageable, damageableTargetSharedContainer.TargetTr);
                });
            
            _behaviorTree
                .FindTask<FindClosestDamageableTarget>(CommonBehaviorTasksNames.FindClosestDamageableTarget)
                .Initialize(combatTargetsProvider, combatTargetHolder)
                .SetSharedVariables(selfGeneralContainer.SelfTransform,
                    damageableTargetSharedContainer.TargetDamageable, damageableTargetSharedContainer.TargetTr);
            
            _behaviorTree
                .FindTask<IsTargetWithinRange>(CommonBehaviorTasksNames.IsTargetWithinRange)
                .SetSharedVariables(selfGeneralContainer.SelfTransform, damageableTargetSharedContainer.TargetTr,
                    weaponStatsProvider);
            
            _behaviorTree
                .FindTask<IsTargetOutOfRangeNotified>(CommonBehaviorTasksNames.IsTargetOutOfRangeNotified)
                .SetNotifier(new BehaviorEventSender(_behaviorTree, CommonBehaviorEventsNames.InterruptCombatActions))
                .SetSharedVariables(selfGeneralContainer.SelfTransform, damageableTargetSharedContainer.TargetTr,
                    weaponStatsProvider);
            
            _behaviorTree
                .FindTask<ProcessActionsInterrupted>(CommonBehaviorTasksNames.ProcessCombatActions)
                .SetupInterruption(false, 
                    new BehaviorEventReceiver(_behaviorTree, CommonBehaviorEventsNames.InterruptCombatActions))
                .Initialize(combatActions.Cast<IBehaviorAction>().ToList());
            
            _behaviorTree
                .FindTask<RotateTo>(CommonBehaviorTasksNames.RotateToDamageable)
                .SetSharedVariables(selfGeneralContainer.SelfTransform, damageableTargetSharedContainer.TargetTr);
        }

        private T GetSharedContainer<T>() where T : SharedVariable
        {
            return (T)_sharedContainers[typeof(T)];
        }
    }
}
