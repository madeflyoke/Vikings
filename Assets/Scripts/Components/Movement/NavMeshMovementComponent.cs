using System;
using Components.Animation;
using Components.Animation.Interfaces;
using Components.Interfaces;
using UniRx;
using UnityEngine.AI;
using Utility;

namespace Components.Movement
{
    public class NavMeshMovementComponent : IEntityComponent, IAnimationCallerHolder
    {
        public AnimationCaller AnimationCaller { get; }
        public NavMeshAgent Agent { get; }
        
        private IDisposable _agentSpeedObserver;

        public NavMeshMovementComponent(NavMeshAgent agent)
        {
            Agent = agent;
            AnimationCaller = new AnimationCaller();
        }

        public void InitializeComponent()
        {
            _agentSpeedObserver = Observable.EveryUpdate()
                .Subscribe(x =>
                {
                    UpdateAnimationValue();
                });
        }
        
        private void UpdateAnimationValue()
        {
            AnimationCaller.CallOnParameterValueChange(AnimatorParametersNames.CurrentVelocity,
                Agent.velocity.magnitude);
        }

        public void Dispose()
        {
            _agentSpeedObserver?.Dispose();
        }
    }
}
