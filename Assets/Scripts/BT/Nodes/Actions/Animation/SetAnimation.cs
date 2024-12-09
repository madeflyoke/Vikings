using BehaviorDesigner.Runtime.Tasks;
using Components.Animation;
using Components.Animation.Interfaces;
using UnityEngine;

namespace BT.Nodes.Actions.Animation
{
    public class SetAnimation : Action
    {
        private IAnimationPlayer _animationPlayer;
        private AnimationClipData _clipData;
        private int _clipDataHash;
        private bool _oneTimed;
        
        public SetAnimation Initialize(IAnimationPlayer animationPlayer, AnimationClipData clipData, bool oneTimed = true)
        {
            _animationPlayer = animationPlayer;
            _clipData = clipData;
            _oneTimed = oneTimed;
            _clipDataHash = _clipData.GetHashCode();
            return this;
        }
        
        public override void OnStart()
        {
            if (_oneTimed && _animationPlayer.GetCurrentPlayedAnimationHash()==_clipDataHash)
            {
                return;
            }

            _animationPlayer.PlayCustomAnimation(_clipData);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
