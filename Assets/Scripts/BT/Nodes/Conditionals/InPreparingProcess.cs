using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BT.Nodes.Conditionals
{
    public class InPreparingProcess : Conditional
    {
        private bool _isReady;
        
        public override TaskStatus OnUpdate()
        {
            return _isReady ? TaskStatus.Failure : TaskStatus.Running;
        }
        
        public void SetReady()
        {
            _isReady = true;
        }
    }
}
