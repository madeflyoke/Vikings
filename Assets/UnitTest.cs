using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using BT.Interfaces;
using BT.Nodes.Actions;
using BT.Nodes.Conditionals;
using BT.Shared.Containers;
using BT.Utility;
using Components.Animation;
using Components.Combat.Actions;
using Components.Health;
using Components.Movement;
using Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Units.Base;
using UnityEngine;
using UnityEngine.AI;

public class UnitTest : SerializedMonoBehaviour
{
    [SerializeField] private List<UnitTest> _enemies;
    
    private IDamageable Damageable;
    public NavMeshAgent _agent;
    public BehaviorTree _behaviorTree;
    [SerializeField] public Animator _animator;
    [OdinSerialize] public List<CombatAction> _attackActions;

    private void Awake()
    {
        Damageable = new HealthComponent(1);
    }

    [Button]
    public void SetDead()
    {
        Damageable.TakeDamage(100);
    }
    
    [Button]
    public void BehaviorOn()
    {
        var animationComponent = new AnimationComponent(_animator, gameObject.AddComponent<AnimationEventsListener>());
        
        _attackActions.ForEach(x =>
        {
            x.Initialize();
            animationComponent.RegisterAnimationCaller(x.AnimationCaller);
        });

        var entity = gameObject.AddComponent<UnitEntity>();
        var movementComponent = new NavMeshMovementComponent(_agent);
        entity.AddEntityComponent(movementComponent);

        
        animationComponent.RegisterAnimationCaller(movementComponent.AnimationCaller);

        MovementSharedContainer movementContainer =
            _behaviorTree.GetCachedVariablesHolder().GetVariable<MovementSharedContainerVariable>().Value;
        
        DamageableTargetSharedContainer damageableTargetSharedContainer =
            _behaviorTree.GetCachedVariablesHolder().GetVariable<DamageableTargetSharedContainerVariable>().Value;
        
        SelfGeneralDataSharedContainer selfGeneralContainer =
            _behaviorTree.GetCachedVariablesHolder().GetVariable<SelfGeneralDataSharedContainerVariable>().Value;
        selfGeneralContainer.SelfTransform.Value = transform;



        _behaviorTree
            .FindTask<ValidateDamageableTarget>(CommonUnitBehaviorTasksNames.ValidateDamageableTarget)
            .SetSharedVariables
                (damageableTargetSharedContainer.TargetDamageable, damageableTargetSharedContainer.TargetTransform);
        
        _behaviorTree
            .FindTask<FindClosestDamageableTarget>(CommonUnitBehaviorTasksNames.FindClosestDamageableTarget)
            .Initialize(_enemies.ToDictionary(x=>x.transform,z=>z.Damageable))
            .SetSharedVariables(selfGeneralContainer.SelfTransform,
                damageableTargetSharedContainer.TargetDamageable, damageableTargetSharedContainer.TargetTransform);
        
        _behaviorTree
            .FindTask<SetClosestNavMeshPoint>(CommonUnitBehaviorTasksNames.SetClosestNavMeshPoint)
            .SetSharedVariables(damageableTargetSharedContainer.TargetTransform, movementContainer.CurrentDestinationPoint);
        
        _behaviorTree
            .FindTask<IsTargetWithinRange>(CommonUnitBehaviorTasksNames.IsTargetWithinRange)
            .SetSharedVariables(selfGeneralContainer.SelfTransform, damageableTargetSharedContainer.TargetTransform, 10f);

        _behaviorTree
            .FindTask<MoveToPoint>(CommonUnitBehaviorTasksNames.MoveToPoint)
            .Initialize(_agent).SetSharedVariables(movementContainer.CurrentDestinationPoint);
        
        _behaviorTree.FindTasks<StopMoving>().ForEach(x=>x.Initialize(_agent));
        
        // _behaviorTree
        //     .FindTask<StopMoving>(CommonUnitBehaviorTasksNames.StopMoving)
        //     .Initialize(_agent);

        _behaviorTree
            .FindTask<ProcessActions>(CommonUnitBehaviorTasksNames.ProcessCombatActions)
            .Initialize(_attackActions.Cast<IBehaviorAction>().ToList());
        
        _behaviorTree
            .FindTask<RotateTo>(CommonUnitBehaviorTasksNames.RotateToDamageable)
            .SetSharedVariables(selfGeneralContainer.SelfTransform, damageableTargetSharedContainer.TargetTransform);
        
        _behaviorTree.EnableBehavior();
    }


    [Button]
    public void SetReady()
    {
        _behaviorTree.FindTask<InPreparingProcess>().SetReady();
    }
}
