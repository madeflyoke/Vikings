using System.Collections.Generic;
using Components.Interfaces;
using Sirenix.Utilities;
using UnityEngine;
using Utility;

namespace Components.Animation
{
   public class AnimationComponent : IEntityComponent
   {
      private readonly AnimationEventsListener _animationEventsListener;
      private readonly Animator _animator;

      public AnimationComponent(Animator animator, AnimationEventsListener eventsListener = null)
      {
         _animator = animator;
         _animationEventsListener = eventsListener;
      }
      
      public void InitializeComponent() { }

      public void SetAnimatorStateSpeedMultiplier(string parameterName, float speed)
      {
         _animator.SetFloat(parameterName,speed);
         Debug.LogWarning("Speed "+speed);
      }
      
      private void PlayAnimation(string name, float transitionDuration = 0.25f)
      {
         _animator.CrossFadeInFixedTime(name, transitionDuration);
      }

      private void PlayCustomAnimation(AnimationCaller caller, AnimationClipData clipData)
      {
         if (_animator.runtimeAnimatorController is AnimatorOverrideController overrider)
         {
            if (clipData.AnimationClip != null)
            {
               overrider[clipData.TargetStateName] = clipData.AnimationClip;
            }
         }

         PlayAnimation(clipData.TargetStateName, clipData.TransitionDuration);
      }

      public void RegisterAnimationCaller(AnimationCaller animationCaller)
      {
         if (animationCaller != null)
         {
            animationCaller.CallOnAnimation += PlayCustomAnimation;
            _animationEventsListener.AnimationEventFired += animationCaller.Callback;
         }
      }
      
      public void RegisterAnimationCallerMany(IEnumerable<AnimationCaller> animationCallers)
      {
         animationCallers.ForEach(RegisterAnimationCaller);
      }
   }
}
