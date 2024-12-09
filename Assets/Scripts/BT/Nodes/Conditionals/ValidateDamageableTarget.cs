using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BT.Shared;
using Components.Movement.Interfaces;
using UnityEngine.AI;

namespace BT.Nodes.Conditionals
{
    public class ValidateDamageableTarget : Conditional
    {
        private SharedDamageable _targetDamageable;
        private SharedTransform _targetTr;
        private IMovementProvider _movementProvider;

        public void SetSharedVariables(SharedDamageable targetDamageable, SharedTransform targetTr)
        {
            _targetDamageable = targetDamageable;
            _targetTr = targetTr;
        }

        public ValidateDamageableTarget Initialize(IMovementProvider movementProvider)
        {
            _movementProvider = movementProvider;
            return this;
        }

        public override TaskStatus OnUpdate()
        {
            return Validate() ? TaskStatus.Success : TaskStatus.Failure;
        }

        private bool Validate()
        {
            return ValidateDamageable()
                   && ValidateNavMeshReachability();
        }

        private bool ValidateDamageable()
        {
            return _targetTr.Value != null
                   && _targetDamageable.Value.IsAlive;
        }

        private bool ValidateNavMeshReachability()
        {
            var samplePositionExists = NavMesh.SamplePosition(_targetTr.Value.position, out NavMeshHit hit, 3f, 1);
            var path = _movementProvider.CalculatePath(hit.position);
            
            return samplePositionExists && path.status==NavMeshPathStatus.PathComplete;
        }
    }
}
