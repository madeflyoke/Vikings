using Components.BT.Actions.Interfaces;

namespace Components.BT.Actions.Containers.Interfaces
{
    public interface IBehaviorActionContainer
    {
        public IBehaviorActionSetup TargetActionSetup { get; set; }
    }
}
