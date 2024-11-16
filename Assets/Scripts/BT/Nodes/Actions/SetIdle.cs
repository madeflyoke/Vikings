using BehaviorDesigner.Runtime.Tasks;
using Components.Animation;
using Components.Animation.Interfaces;
using Utility;

namespace BT.Nodes.Actions
{
    public class SetIdle : Action, IAnimationCallerHolder
    {
        public AnimationCaller AnimationCaller { get; private set; }
        
        public SetIdle Initialize()
        {
            AnimationCaller = new AnimationCaller();
            return this;
        }
        
        public override void OnStart()
        {
            AnimationCaller.CallOnAnimationWithCallback?.Invoke(AnimationCaller,new AnimationClipData(targetStateName:AnimatorStatesNames.Idle),null);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
