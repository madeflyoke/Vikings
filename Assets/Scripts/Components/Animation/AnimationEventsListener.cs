using System;
using Components.Animation.Enums;
using UnityEngine;

namespace Components.Animation
{
    [RequireComponent(typeof(Animator))]
    public class AnimationEventsListener : MonoBehaviour
    {
        public event Action<AnimationEventType> AnimationEventFired;
        
        public void OnAnimationStart()
        {
            AnimationEventFired?.Invoke(AnimationEventType.START);
        }

        public void OnAnimationEnd()
        {
            AnimationEventFired?.Invoke(AnimationEventType.END);
        }

        public void HitStart()
        {
            AnimationEventFired?.Invoke(AnimationEventType.HITSTART);
        }

        public void HitEnd()
        {
            AnimationEventFired?.Invoke(AnimationEventType.HITEND);
        }
    }
}