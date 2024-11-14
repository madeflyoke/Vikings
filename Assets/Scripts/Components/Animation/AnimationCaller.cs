using System;

namespace Components.Animation
{
    public class AnimationCaller
    {
        public Action<AnimationCaller, AnimationClipData, Action> CallOnAnimationWithCallback { get; set; }
        public Action<string, float> CallOnParameterValueChange { get; set; }

        public AnimationEventsListener AnimationsEventsListener { get; private set; }
        
        public void AttachAnimationEventsListener(AnimationEventsListener eventsListener)
        {
            AnimationsEventsListener = eventsListener;
        }
    }
}
