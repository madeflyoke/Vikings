using System.Collections.Generic;
using Components.Animation.Interfaces;
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

      public void SetParameterValue(string parameterName, float value)
      {
         _animator.SetFloat(parameterName,value);
      }

      private void PlayAnimation(string name, int layerIndex =0, float transitionDuration = 0.25f)
      {
         if (layerIndex!=0)
         {
            _animator.CrossFadeInFixedTime(name, transitionDuration, layer: layerIndex);
         }
         else
         {
            _animator.CrossFadeInFixedTime(name, transitionDuration);
         }
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

         PlayAnimation(clipData.TargetStateName, 
            clipData.FullBodyLayer? AnimatorLayersIndexes.FullBodyLayer: 0,
            clipData.TransitionDuration);
      }

      public void RegisterAnimationCaller(IAnimationCallerHolder animationCallerHolder)
      {
         if (animationCallerHolder != null)
         {
            animationCallerHolder.AnimationCaller.CallOnAnimation += PlayCustomAnimation; //handle unsubscribing?
            animationCallerHolder.AnimationCaller.CallOnParameterValueChange += SetParameterValue;
            animationCallerHolder.AnimationCaller.AttachAnimationEventsListener(_animationEventsListener);
         }
      }
      
      public void RegisterAnimationCallerMany(IEnumerable<IAnimationCallerHolder> animationCallersHolders)
      {
         animationCallersHolders.ForEach(RegisterAnimationCaller);
      }

      public void Dispose()
      {
      }
   }
}
