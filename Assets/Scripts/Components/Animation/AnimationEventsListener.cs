using System;
using Components.Animation.Enums;
using UnityEngine;

namespace Components.Animation
{
    [RequireComponent(typeof(Animator))]
    public class AnimationEventsListener : MonoBehaviour
    {
        public event Action<AnimationEventType> AnimationEventFired;
        
        public void OnHitStart()
        {
            AnimationEventFired?.Invoke(AnimationEventType.HITSTART);
        }

        public void OnHitEnd()
        {
            AnimationEventFired?.Invoke(AnimationEventType.HITEND);
        }
    }
}