using BehaviorDesigner.Runtime.Tasks;
using Components.Animation;
using Components.Animation.Interfaces;
using Components.Movement.Interfaces;
using UnityEngine.AI;
using Utility;

namespace BT.Nodes.Actions
{
    public class SetDeath : Action
    {
        private IAnimationPlayer _animationPlayer;
        private IMovementProvider _movementProvider;
        
        public SetDeath Initialize(IMovementProvider movementProvider,IAnimationPlayer animationPlayer)
        {
            _animationPlayer = animationPlayer;
            _movementProvider = movementProvider;
            return this;
        }
        
        public override void OnStart()
        {
            _animationPlayer.PlayCustomAnimation(new AnimationClipData(targetStateName:AnimationStatesNames.Death));
            _movementProvider.StopMovement();
            _movementProvider.SetRelatedComponentActive(false);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }
    }
}
