using System;
using System.Collections.Generic;
using Components.Animation.Interfaces;
using Components.Interfaces;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using Utility;

namespace Components.Animation
{
   public class AnimationComponent : IEntityComponent, IAnimationCallerRegister
   {
      private readonly AnimationEventsListener _animationEventsListener;
      private readonly Animator _animator;
      private CompositeDisposable _compositeDisposable;
      
      public AnimationComponent(Animator animator, AnimationEventsListener eventsListener = null)
      {
         _animator = animator;
         _animationEventsListener = eventsListener;
      }

      public void InitializeComponent()
      {
         _compositeDisposable = new CompositeDisposable();
      }

      private void SetParameterValue(string parameterName, float value)
      {
         _animator.SetFloat(parameterName,value);
      }

      private float GetParameterValue(string parameterName)
      {
         return _animator.GetFloat(parameterName);
      }

      private void CheckAnimationEnd(AnimationClipData clipData, Action onComplete)
      {
         var speedMultiplier =GetParameterValue(AnimatorParametersNames.GetCorrespondingParameter(clipData.TargetStateName));
         
         Observable
            .Timer(TimeSpan.FromSeconds(clipData.AnimationClip.length / speedMultiplier))
            .Subscribe(_ => onComplete?.Invoke()).AddTo(_compositeDisposable);
      }
      
      private void PlayAnimation(string name, float transitionDuration = 0.25f)
      {
         _animator.CrossFadeInFixedTime(name, transitionDuration);
      }

      private void PlayCustomAnimation(AnimationCaller caller, AnimationClipData clipData, Action callBack)
      {
         if (_animator.runtimeAnimatorController is AnimatorOverrideController overrider)
         {
            if (clipData.AnimationClip != null)
            {
               overrider[clipData.TargetStateName] = clipData.AnimationClip;
            }
         }

         PlayAnimation(clipData.TargetStateName,
            clipData.TransitionDuration);
         
         if (callBack!=null)
         {
            CheckAnimationEnd(clipData, callBack);
         }
      }

      public void RegisterAnimationCaller(IAnimationCallerHolder animationCallerHolder)
      {
         if (animationCallerHolder != null)
         {
            animationCallerHolder.AnimationCaller.CallOnAnimationWithCallback += PlayCustomAnimation; //handle unsubscribing?
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
         _compositeDisposable?.Dispose();
      }
   }
}
