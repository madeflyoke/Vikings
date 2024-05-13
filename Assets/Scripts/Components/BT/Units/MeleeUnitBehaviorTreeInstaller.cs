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
using Components.Combat;
using Components.Combat.Actions;
using Components.Movement;
using Interfaces;
using Units.Components;
using UnityEngine.AI;

namespace Components.BT.Units
{
    public class MeleeUnitBehaviorTreeInstaller : IBehaviorTreeInstaller
    {
        private BehaviorTree _behaviorTree;
        private Dictionary<Type, SharedVariable> _sharedContainers;
        
        public IBehaviorTreeInstaller Install(BehaviorTree behaviorTree, BehaviorTreeCachedVariablesHolder cachedVariablesHolder, IReadOnlyEntity entity,
        out Action startTrigger)
        {
            _behaviorTree = behaviorTree;
            SetupSharedContainers(cachedVariablesHolder, entity.GetEntityComponent<EntityHolder>());
            SetupNavMeshMovementTasks(entity.GetEntityComponent<NavMeshMovementComponent>().Agent);
            SetupCombatTasks(entity.GetEntityComponent<CombatComponent>().CombatActions, 
                UnitsTeamSpawnersHolder.Instance.GetOpponentsTargetsProvider(entity.GetEntityComponent<UnitTagHolder>().Team));
            
            startTrigger = ()=>_behaviorTree.FindTask<InPreparingProcess>().SetReady();
            return this;
        }
        
        private void SetupSharedContainers(BehaviorTreeCachedVariablesHolder cachedVariablesHolder, EntityHolder entityHolder)
        {
            _sharedContainers = new Dictionary<Type, SharedVariable>();

            SetContainer<MovementSharedContainerVariable>();
            SetContainer<DamageableTargetSharedContainerVariable>();
            SetContainer<SelfGeneralDataSharedContainerVariable>().Value.SelfTransform = entityHolder.SelfTransform;
            
            T SetContainer<T>() where T: SharedVariable
            {
                T container = cachedVariablesHolder.GetVariable<T>();
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
                .FindTask<SetClosestNavMeshPoint>(MeleeUnitBehaviorTasksNames.SetClosestNavMeshPoint)
                .SetSharedVariables(damageableTargetSharedContainer.TargetTr, 
                    movementSharedContainer.CurrentDestinationPoint);
        }

        private void SetupCombatTasks(IEnumerable<CombatAction> combatActions, ICombatTargetsProvider combatTargetsProvider)
        {
            var damageableTargetSharedContainer = GetSharedContainer<DamageableTargetSharedContainerVariable>().Value;
            var selfGeneralContainer = GetSharedContainer<SelfGeneralDataSharedContainerVariable>().Value;

            _behaviorTree
                .FindTask<ValidateDamageableTarget>(MeleeUnitBehaviorTasksNames.ValidateDamageableTarget)
                .SetSharedVariables
                    (damageableTargetSharedContainer.Damageable, damageableTargetSharedContainer.TargetTr);
            
            _behaviorTree
                .FindTask<FindClosestDamageableTarget>(MeleeUnitBehaviorTasksNames.FindClosestDamageableTarget)
                .Initialize(combatTargetsProvider)
                .SetSharedVariables(selfGeneralContainer.SelfTransform,
                    damageableTargetSharedContainer.Damageable, damageableTargetSharedContainer.TargetTr);
            
            _behaviorTree
                .FindTask<IsTargetWithinRange>(MeleeUnitBehaviorTasksNames.IsTargetWithinRange)
                .SetSharedVariables(selfGeneralContainer.SelfTransform, damageableTargetSharedContainer.TargetTr,
                    10f); //TODO 10
            
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
