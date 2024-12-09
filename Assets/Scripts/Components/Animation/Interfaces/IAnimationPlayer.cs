using System;
using UniRx;

namespace Components.Animation.Interfaces
{
    public interface IAnimationPlayer
    {
        public AnimationEventsListener AnimationEventsListener { get; }
        
        public void PlayCustomAnimation(AnimationClipData clipData);
        public IDisposable PlayCustomAnimation(AnimationClipData clipData, Action callBack);
        public int GetCurrentPlayedAnimationHash();
    }
}
