using System;
using Components.Animation.Enums;

namespace Components.Animation
{
    public class AnimationCaller
    {
        public Action<AnimationCaller, AnimationClipData> CallOnAnimation { get; set; }
        public Action<AnimationEventType> Callback { get; set; }
    }
}
