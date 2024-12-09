using Components.Animation.Interfaces;
using Components.BT.Actions.Containers.Interfaces;
using Components.BT.Actions.Interfaces;

namespace Components.BT.Actions.Containers
{
    public class AnimatedBehaviorActionContainer : IBehaviorActionContainer
    {
        public IBehaviorActionSetup TargetActionSetup { get; set; }
        public IAnimationPlayer AnimationPlayer;
    }
}
