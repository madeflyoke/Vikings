using System;
using Components.Settings.Interfaces;
using Components.View;

namespace Components.Settings
{
    [Serializable]
    public class ModelHolderSettings : IComponentSettings
    {
        public ModelHolder ModelHolderPrefab;
        
#if UNITY_EDITOR
        public void OnManualValidate(){}
#endif
    }
}