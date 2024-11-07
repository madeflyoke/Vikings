using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BT.Shared;
using UnityEngine.AI;

namespace BT.Nodes.Conditionals
{
    public class ValidateDamageableTarget : Conditional
    {
        private SharedDamageable _targetDamageable;
        private SharedTransform _targetTr;
        
        public void SetSharedVariables(SharedDamageable targetDamageable, SharedTransform targetTr)
        {
            _targetDamageable = targetDamageable;
            _targetTr = targetTr;
        }

        public override TaskStatus OnUpdate()
        {
            return Validate() ? TaskStatus.Success : TaskStatus.Failure;
        }

        private bool Validate()
        {
            return _targetTr.Value!=null
                   && _targetDamageable.Value.IsAlive
                   && ValidateNavMeshReachability();
        }

        private bool ValidateNavMeshReachability()
        {
            return NavMesh.SamplePosition(_targetTr.Value.position, out NavMeshHit _, 3f, 1);
        }
    }
}
