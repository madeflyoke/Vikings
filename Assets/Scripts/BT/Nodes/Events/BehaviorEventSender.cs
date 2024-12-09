using BehaviorDesigner.Runtime;

namespace BT.Nodes.Events
{
    public class BehaviorEventSender
    {
        private readonly string _targetEventName;
        private readonly BehaviorTree _relatedBt;
        
        public BehaviorEventSender(BehaviorTree relatedBt, string targetEventName)
        {
            _targetEventName = targetEventName;
            _relatedBt = relatedBt;
        }

        public void Send()
        {
            _relatedBt.SendEvent(_targetEventName);
        }
    }
}
