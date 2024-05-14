using BehaviorDesigner.Runtime;
using Components.Settings.Interfaces;

namespace Components.Settings
{
    public class BehaviorTreeComponentSettings : IComponentSettings
    {
        public ExternalBehaviorTree ExternalBehaviorTree;
        
#if UNITY_EDITOR
        public void OnManualValidate(){}
#endif
    }
}
