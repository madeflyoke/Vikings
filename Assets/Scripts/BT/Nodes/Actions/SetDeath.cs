using BehaviorDesigner.Runtime.Tasks;
using Components.Animation;
using Components.Animation.Interfaces;
using Utility;

namespace BT.Nodes.Actions
{
    public class SetDeath : Action, IAnimationCallerHolder
    {
        public AnimationCaller AnimationCaller { get; private set; }
        
        public SetDeath Initialize()
        {
            AnimationCaller = new AnimationCaller();
            return this;
        }
        
        public override void OnStart()
        {
            AnimationCaller.CallOnAnimationWithCallback?.Invoke(AnimationCaller,new AnimationClipData(targetStateName:AnimatorStatesNames.Death),null);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }
    }
}
