using System;
using BehaviorDesigner.Runtime.Tasks;
using BT.Interfaces;
using Components.Animation;
using Components.Animation.Enums;
using UnityEngine;

namespace Components.Combat.Actions
{
    [Serializable]
    public abstract class CombatAction : IBehaviorAction
    {
        public AnimationCaller AnimationCaller { get; private set; }
        
        [field: SerializeField] public AnimationClipData AnimationClipData { get; }

        public void Initialize()
        {
            AnimationCaller = new AnimationCaller();
            AnimationCaller.Callback += OnAnimationCallback;
        }
        
        protected virtual void OnAnimationCallback(AnimationEventType eventType) { }
       
        public abstract TaskStatus GetCurrentStatus();
        public abstract void Execute();
    }
}