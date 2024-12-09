using System;
using BehaviorDesigner.Runtime.Tasks;
using Components.Animation;
using Components.Animation.Interfaces;
using Components.BT.Actions.Containers;
using Components.BT.Actions.Containers.Interfaces;
using Components.BT.Actions.Interfaces;
using Components.BT.Actions.Setups;
using UniRx;
using Utility;

namespace Components.BT.Actions
{
    public class IdleTimedAction : IBehaviorAction, IBehaviorActionInjectedByContainer
    {
        public TaskStatus CurrentStatus { get; private set; }
        
        private IAnimationPlayer _animationPlayer;
        private IdleTimedActionSetup _setup;
        private IDisposable _disposable;
        private bool _executed;
    
        public void Construct(IBehaviorActionContainer container)
        {
            var specifiedContainer = container as AnimatedBehaviorActionContainer;
            _animationPlayer = specifiedContainer.AnimationPlayer;
            _setup = specifiedContainer.TargetActionSetup as IdleTimedActionSetup;
        }
        
        public void Execute()
        {
            if (_setup.OneTimed && _executed)
            {
                CurrentStatus = TaskStatus.Success;
                return;
            }
            CurrentStatus = TaskStatus.Running;
            _animationPlayer.PlayCustomAnimation(new AnimationClipData(targetStateName: AnimationStatesNames.Idle));
            _disposable = Observable.Timer(TimeSpan.FromSeconds(_setup.Duration)).Subscribe(_=>SetCompleted());
        }

        private void SetCompleted()
        {
            CurrentStatus = TaskStatus.Success;
            _executed = true;
        }
        
        public void Stop()
        {
            CurrentStatus = TaskStatus.Inactive;
            _disposable?.Dispose();
            _executed = false;
        }

        public void NotifyInterrupt()
        {
            if (_setup.CanBeInterrupted)
            {
                _disposable?.Dispose();
                SetCompleted();
            }
        }
    }
}
