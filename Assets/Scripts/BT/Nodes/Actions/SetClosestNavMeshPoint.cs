using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace BT.Nodes.Actions
{
    public class SetClosestNavMeshPoint : Action
    {
        private SharedVector3 _sourcePoint;

        public void SetSharedVariables(SharedVector3 sourcePoint)
        {
            _sourcePoint = sourcePoint;
        }

        public override TaskStatus OnUpdate()
        {
            return SetClosestDestinationPoint() ? TaskStatus.Success : TaskStatus.Failure;
        }

        private bool SetClosestDestinationPoint()
        {
            if (NavMesh.SamplePosition(_sourcePoint.Value, out NavMeshHit hit, 3f, 1))
            {
                _sourcePoint.Value = hit.position;
                return true;
            }
            return false;
        }
    }
}
