using System;
using Components.Animation.Enums;

namespace Components.Animation
{
    public class AnimationCaller
    {
        public Action<AnimationCaller, AnimationClipData> CallOnAnimation { get; set; }

        public AnimationEventsListener AnimationsEventsListener { get; private set; }
        
        public void AttachAnimationEventsListener(AnimationEventsListener eventsListener)
        {
            AnimationsEventsListener = eventsListener;
        }
    }
}
