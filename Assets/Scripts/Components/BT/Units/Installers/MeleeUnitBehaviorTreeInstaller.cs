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
using CombatTargetsProviders.Interfaces;
using Components.BT.Interfaces;
using Components.BT.Units.Installers.Data;
using Components.Combat.Actions;
using Components.Combat.Interfaces;
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

            SetupSharedContainers(installerData.EntityHolder);
            SetupNavMeshMovementTasks(installerData.Agent);

            SetupOpponentTargetTasks(installerData.CombatActions, installerData.CombatTargetsProvider,
                    installerData.CombatStatsCopyProvider, installerData.Agent, installerData.CombatTargetHolder);
            
            installerData.BehaviorTreeStarter.BehaviorTreeStartEvent += ()=> _behaviorTree.FindTask<InPreparingProcess>().SetReady(); 
        }

        private void SetupSharedContainers(EntityHolder entityHolder)
        {
            _cachedVariablesHolder = new BehaviorTreeCachedVariablesHolder(_behaviorTree);
            _sharedContainers = new Dictionary<Type, SharedVariable>();
            
            SetContainer<MovementSharedContainerVariable>();
            SetContainer<DamageableTargetSharedContainerVariable>();
            SetContainer<SelfGeneralDataSharedContainerVariable>().Value.SelfTransform = entityHolder.SelfTransform;
            
            T SetContainer<T>() where T: SharedVariable
            {
                T container = _cachedVariablesHolder.GetVariable<T>();
                _sharedContainers.Add(container.GetType(), container);
                return container;
            }
        }

        private void SetupNavMeshMovementTasks(NavMeshAgent agent)
        {
            var movementSharedContainer = GetSharedContainer<MovementSharedContainerVariable>().Value;
            var damageableTargetSharedContainer = GetSharedContainer<DamageableTargetSharedContainerVariable>().Value;

            _behaviorTree
                .FindTask<MoveToPoint>(MeleeUnitBehaviorTasksNames.MoveToPoint)
                .Initialize(agent).SetSharedVariables(movementSharedContainer.CurrentDestinationPoint);
            
            _behaviorTree.FindTasks<StopMoving>().ForEach(x=>x.Initialize(agent));
            
            _behaviorTree
                .FindTasks<SetClosestNavMeshPoint>().ForEach(x=>
                x.SetSharedVariables(damageableTargetSharedContainer.TargetTr, 
                    movementSharedContainer.CurrentDestinationPoint));
        }

        private void SetupOpponentTargetTasks(IEnumerable<CombatAction> combatActions, ICombatTargetsProvider combatTargetsProvider,
            ICombatStatsCopyProvider combatStatsCopyProvider, NavMeshAgent agent, ICombatTargetHolder combatTargetHolder)
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
                    combatStatsCopyProvider.GetCombatStatsCopy().AttackRange);
            
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
