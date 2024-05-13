using System;
using Components.Animation;
using Components.Interfaces;
using UniRx;
using UnityEngine.AI;
using Utility;

namespace Components.Movement
{
    public class NavMeshMovementComponent : IEntityComponent, IDisposable
    {
        public AnimationCaller AnimationCaller { get; }
        public NavMeshAgent Agent { get; }
        
        private bool IsAgentMoving => Agent.velocity.magnitude > 0f;
        
        private IDisposable _agentSpeedObserver;

        public NavMeshMovementComponent(NavMeshAgent agent)
        {
            Agent = agent;
            AnimationCaller = new AnimationCaller();
            Initialize();
        }

        private void Initialize()
        {
            _agentSpeedObserver = this.ObserveEveryValueChanged(x => x.IsAgentMoving).Skip(1)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        OnAgentMoving();
                    }
                    else
                    {
                        OnAgentStopped();
                    }
                });
        }
        
        private void OnAgentMoving()
        {
            AnimationCaller.CallOnAnimation?.Invoke(AnimationCaller, new AnimationClipData(targetStateName: AnimatorStatesNames.Moving));
        }
        
        private void OnAgentStopped()
        {
            AnimationCaller.CallOnAnimation?.Invoke(AnimationCaller, new AnimationClipData(targetStateName: AnimatorStatesNames.Idle));
        }

        public void Dispose()
        {
            _agentSpeedObserver?.Dispose();
        }
    }
}
