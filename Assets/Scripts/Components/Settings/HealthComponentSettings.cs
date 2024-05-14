using Components.Settings.Interfaces;

namespace Components.Settings
{
    public class HealthComponentSettings : IComponentSettings
    {
        public int BaseHealth = 100;
        
#if UNITY_EDITOR
        public void OnManualValidate(){}
#endif
    }
}
