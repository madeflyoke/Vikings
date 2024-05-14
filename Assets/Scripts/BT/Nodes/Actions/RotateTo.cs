using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BT.Nodes.Actions
{
    public class RotateTo : Action
    {
        private SharedTransform _selfTransform;
        private SharedTransform _targetTransform;

        public void SetSharedVariables(SharedTransform selfTransform, SharedTransform targetTransform)
        {
            _selfTransform = selfTransform;
            _targetTransform = targetTransform;
        }
        
        public override TaskStatus OnUpdate()
        {
            _selfTransform.Value.rotation = Quaternion.LookRotation(_targetTransform.Value.position - _selfTransform.Value.position);
            return TaskStatus.Success;
        }
    }
}
