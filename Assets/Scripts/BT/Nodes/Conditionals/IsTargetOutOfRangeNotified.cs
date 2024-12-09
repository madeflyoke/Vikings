using BehaviorDesigner.Runtime.Tasks;
using BT.Nodes.Events;

namespace BT.Nodes.Conditionals
{
    public class IsTargetOutOfRangeNotified : IsTargetWithinRange
    {
        private BehaviorEventSender _notifier;
        
        public IsTargetWithinRange SetNotifier(BehaviorEventSender notifier)
        {
            _notifier = notifier;
            return this;
        }

        public override TaskStatus OnUpdate()
        {
            if (base.OnUpdate()==TaskStatus.Failure)
            {
                _notifier.Send();
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }
    }
}