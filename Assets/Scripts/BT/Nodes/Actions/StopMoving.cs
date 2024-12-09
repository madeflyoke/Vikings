using BehaviorDesigner.Runtime.Tasks;
using Components.Movement.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace BT.Nodes.Actions
{
    public class StopMoving : Action
    {
        private IMovementProvider _movementProvider;
        
        public void Initialize(IMovementProvider movementProvider)
        {
            _movementProvider = movementProvider;
        }

        public override TaskStatus OnUpdate()
        {
            _movementProvider.StopMovement();
            return TaskStatus.Success;
        }
    }
}
