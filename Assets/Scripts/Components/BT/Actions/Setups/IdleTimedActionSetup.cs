using Components.BT.Actions.Attributes;
using Components.BT.Actions.Interfaces;

namespace Components.BT.Actions.Setups
{
    [BehaviorActionVariant(ActionVariant = typeof(IdleTimedAction))]
    public class IdleTimedActionSetup : IBehaviorActionSetup
    {
        public float Duration = 0.5f;
        public bool CanBeInterrupted = true;
        public bool OneTimed;
    }
}
