using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using BT.Interfaces;
using BT.Nodes.Actions;
using BT.Nodes.Conditionals;
using BT.Shared.Containers;
using BT.Tools;
using BT.Utility;
using Combat.CombatTargetsProviders.Interfaces;
using Components.Animation;
using Components.Animation.Interfaces;
using Components.BT.Interfaces;
using Components.BT.Units.Installers.Data;
using Components.Combat.Actions;
using Components.Combat.Interfaces;
using Components.Health;
using Interfaces;
using UnityEngine.AI;

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
            
            SetupSelfTasks(installerData.AnimationsRegister, installerData.Agent);
            SetupNavMeshMovementTasks(installerData.Agent);
            SetupOpponentTargetTasks(installerData.CombatActions, installerData.CombatTargetsProvider,
                    installerData.CombatStatsProvider, installerData.Agent, installerData.CombatTargetHolder);
            
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

        private void SetupSelfTasks(IAnimationCallerRegister animationRegister, NavMeshAgent agent)
        {
            var selfDataContainer = GetSharedContainer<SelfGeneralDataSharedContainerVariable>().Value;
            
            _behaviorTree
                .FindTask<SelfValidate>()
                .SetSharedVariables(selfDataContainer.SelfDamageable);

            _behaviorTree.FindTasks<SetIdle>().ForEach(x =>
            {
                x.Initialize();
                animationRegister.RegisterAnimationCaller(x);
            });
            
            _behaviorTree.FindTasks<SetDeath>().ForEach(x =>
            {
                x.Initialize(agent);
                animationRegister.RegisterAnimationCaller(x);
            });
        }

        private void SetupNavMeshMovementTasks(NavMeshAgent agent)
        {
            var movementSharedContainer = GetSharedContainer<MovementSharedContainerVariable>().Value;
            var damageableTargetSharedContainer = GetSharedContainer<DamageableTargetSharedContainerVariable>().Value;
            var selfContainer = GetSharedContainer<SelfGeneralDataSharedContainerVariable>().Value;

            _behaviorTree
                .FindTask<MoveToPoint>(MeleeUnitBehaviorTasksNames.MoveToPoint)
                .Initialize(agent).SetSharedVariables(movementSharedContainer.CurrentDestinationPoint);
            
            _behaviorTree.FindTasks<StopMoving>().ForEach(x=>x.Initialize(agent));
            
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

        private void SetupOpponentTargetTasks(IEnumerable<CombatAction> combatActions, ICombatTargetsProvider combatTargetsProvider,
            ICombatStatsProvider combatStatsProvider, NavMeshAgent agent, ICombatTargetHolder combatTargetHolder)
        {
            var damageableTargetSharedContainer = GetSharedContainer<DamageableTargetSharedContainerVariable>().Value;
            var selfGeneralContainer = GetSharedContainer<SelfGeneralDataSharedContainerVariable>().Value;

            _behaviorTree
                .FindTasks<ValidateDamageableTarget>()
                .ForEach(x =>
                {
                    x.Initialize(agent)
                        .SetSharedVariables
                            (damageableTargetSharedContainer.TargetDamageable, damageableTargetSharedContainer.TargetTr);
                });
            
            _behaviorTree
                .FindTask<FindClosestDamageableTarget>(MeleeUnitBehaviorTasksNames.FindClosestDamageableTarget)
                .Initialize(combatTargetsProvider, combatTargetHolder)
                .SetSharedVariables(selfGeneralContainer.SelfTransform,
                    damageableTargetSharedContainer.TargetDamageable, damageableTargetSharedContainer.TargetTr);
            
            _behaviorTree
                .FindTask<IsTargetWithinRange>(MeleeUnitBehaviorTasksNames.IsTargetWithinRange)
                .SetSharedVariables(selfGeneralContainer.SelfTransform, damageableTargetSharedContainer.TargetTr,
                    combatStatsProvider);
            
            _behaviorTree
                .FindTask<ProcessActions>(MeleeUnitBehaviorTasksNames.ProcessCombatActions)
                .Initialize(combatActions.Cast<IBehaviorAction>().ToList());
            
            _behaviorTree
                .FindTask<RotateTo>(MeleeUnitBehaviorTasksNames.RotateToDamageable)
                .SetSharedVariables(selfGeneralContainer.SelfTransform, damageableTargetSharedContainer.TargetTr);
        }

        private T GetSharedContainer<T>() where T : SharedVariable
        {
            return (T)_sharedContainers[typeof(T)];
        }
    }
}
