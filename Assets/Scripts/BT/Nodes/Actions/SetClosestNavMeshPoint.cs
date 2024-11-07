using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace BT.Nodes.Actions
{
    public class SetClosestNavMeshPoint : Action
    {
        private SharedTransform _originalTransform;
        private SharedVector3 _closestPoint;

        public void SetSharedVariables(SharedTransform originalTransform ,SharedVector3 resultPoint)
        {
            _originalTransform = originalTransform;
            _closestPoint = resultPoint;
        }
        
        public override TaskStatus OnUpdate()
        {
            return SetClosestDestinationPoint()? TaskStatus.Success : TaskStatus.Failure;
        }
        
        private bool SetClosestDestinationPoint()
        {
            if (NavMesh.SamplePosition(_originalTransform.Value.position, out NavMeshHit hit, 3f, 1))
            {
                _closestPoint.Value = hit.position;
                return true;
            }

            return false;
        }
    }
}
