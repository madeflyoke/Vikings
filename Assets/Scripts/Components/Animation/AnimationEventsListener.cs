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
            Debug.LogWarning("ANIMATION START");
            AnimationEventFired?.Invoke(AnimationEventType.START);
        }

        public void OnAnimationEnd()
        {
            Debug.LogWarning("ANIMATION END");
            AnimationEventFired?.Invoke(AnimationEventType.END);
        }
    }
}