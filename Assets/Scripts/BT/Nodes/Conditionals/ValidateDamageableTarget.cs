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
        private NavMeshAgent _agent;

        public void SetSharedVariables(SharedDamageable targetDamageable, SharedTransform targetTr)
        {
            _targetDamageable = targetDamageable;
            _targetTr = targetTr;
        }

        public ValidateDamageableTarget Initialize(NavMeshAgent agent)
        {
            _agent = agent;
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
            var path = new NavMeshPath();
            _agent.CalculatePath(hit.position, path);

            return samplePositionExists && path.status==NavMeshPathStatus.PathComplete;
        }
    }
}
