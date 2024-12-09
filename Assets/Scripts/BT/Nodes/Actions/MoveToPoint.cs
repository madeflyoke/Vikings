using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using Components.Movement.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace BT.Nodes.Actions
{
    public class MoveToPoint : Action
    {
        private SharedVector3 _targetPoint;
        private IMovementProvider _movementProvider;

        public void SetSharedVariables(SharedVector3 targetPoint)
        {
            _targetPoint = targetPoint;
        }
        
        public MoveToPoint Initialize(IMovementProvider movementProvider)
        {
            _movementProvider = movementProvider;
            return this;
        }

        public override void OnStart()
        {
            _movementProvider.StartMovement();
        }

        public override TaskStatus OnUpdate()
        {
            _movementProvider.SetMovementPoint(_targetPoint.Value);
            return TaskStatus.Success;
        }
    }
}
