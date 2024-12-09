using System;
using Components.Animation.Interfaces;
using Components.Interfaces;
using UniRx;
using UnityEngine;
using Utility;

namespace Components.Animation
{
   public class AnimationComponent : IEntityComponent, IAnimationPlayer, IAnimatorValueChanger
   {
      public AnimationEventsListener AnimationEventsListener { get; private set; }
      private readonly Animator _animator;
      private CompositeDisposable _compositeDisposable;
      private int _currentPlayedClipHash;
      
      public AnimationComponent(Animator animator, AnimationEventsListener eventsListener)
      {
         _animator = animator;
         AnimationEventsListener = eventsListener;
      }

      public void InitializeComponent()
      {
         _compositeDisposable = new CompositeDisposable();
      }

      public void SetParameterValue(string parameterName, float value)
      {
         _animator.SetFloat(parameterName,value);
      }

      public float GetParameterValue(string parameterName)
      {
         return _animator.GetFloat(parameterName);
      }
      
      public void PlayCustomAnimation(AnimationClipData clipData)
      {
         if (_animator.runtimeAnimatorController is AnimatorOverrideController overrider)
         {
            if (clipData.AnimationClip != null && overrider[clipData.TargetStateName]!=clipData.AnimationClip)
            {
               overrider[clipData.TargetStateName] = clipData.AnimationClip;
            }
         }
         
         PlayAnimationInternal(clipData);
      }
      
      public IDisposable PlayCustomAnimation(AnimationClipData clipData, Action callBack)
      {
         PlayCustomAnimation(clipData);
         return CheckAnimationEnd(clipData, callBack);
      }
      
      private IDisposable CheckAnimationEnd(AnimationClipData clipData, Action callBack)
      {
         var speedMultiplier =GetParameterValue(AnimatorParametersNames.GetCorrespondingParameter(clipData.TargetStateName));
         return Observable
            .Timer(TimeSpan.FromSeconds(clipData.AnimationClip.length / speedMultiplier))
            .Subscribe(_ =>
            {
               Debug.LogWarning("callback");
               callBack?.Invoke();
            }).AddTo(_compositeDisposable);
      }
      
      private void PlayAnimationInternal(AnimationClipData clipData)
      {
         if (_animator.IsInTransition(0))
         {
            _animator.PlayInFixedTime(clipData.TargetStateName, 0, 0);
         }
         else
         {
            _animator.CrossFadeInFixedTime(clipData.TargetStateName, clipData.TransitionDuration);
         }
         _currentPlayedClipHash = clipData.GetHashCode();
      }

      public int GetCurrentPlayedAnimationHash()
      {
         return _currentPlayedClipHash;
      }
      
      public void Dispose()
      {
         _compositeDisposable?.Dispose();
      }
   }
}
