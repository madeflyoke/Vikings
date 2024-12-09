using BehaviorDesigner.Runtime;

namespace BT.Nodes.Events
{
    public class BehaviorEventReceiver
    {
        private readonly string _targetEventName;
        private readonly BehaviorTree _relatedBt;
        
        public BehaviorEventReceiver(BehaviorTree relatedBt, string targetEventName)
        {
            _targetEventName = targetEventName;
            _relatedBt = relatedBt;
        }

        public void Register(System.Action action)
        {
            _relatedBt.RegisterEvent(_targetEventName, action);
        }

        public void UnRegister(System.Action action)
        {
            _relatedBt.UnregisterEvent(_targetEventName, action);
        }
    }
}