using BehaviorDesigner.Runtime.Tasks;
using BT.Nodes.Events;

namespace BT.Nodes.Actions
{
    public class ProcessActionsInterrupted : ProcessActions
    {
        private BehaviorEventReceiver _additionalInterruptSource;
        private bool _instantInterruption;
        private bool _interrupted;
        
        public ProcessActions SetupInterruption(bool instantInterruption, BehaviorEventReceiver additionalInterruptSource)
        {
            _additionalInterruptSource = additionalInterruptSource;
            _instantInterruption = instantInterruption;
            return this;
        }
        
        public override void OnStart()
        {
            _additionalInterruptSource.Register(OnInterrupted);
            base.OnStart();
        }
        
        public override TaskStatus OnUpdate()
        {
            if (_interrupted)
            {
                if (_instantInterruption)
                {
                    return TaskStatus.Failure;
                }
                
                if (Actions[CurrentActionIndex].CurrentStatus == TaskStatus.Success) //wait for last action be performed
                {
                    return TaskStatus.Failure;
                }

                return TaskStatus.Running;
            }

            return base.OnUpdate();
        }
        
        private void OnInterrupted()
        {
            _interrupted = true;
            Actions[CurrentActionIndex].NotifyInterrupt();
        }
        
        public override void OnEnd()
        {
            base.OnEnd();
            _additionalInterruptSource.UnRegister(OnInterrupted);
            _interrupted = false;
        }
    }
}
