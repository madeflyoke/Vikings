using Components.Settings;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Units.Enums;
using UnityEngine;

namespace Units.Configs
{
    [CreateAssetMenu(fileName = "UnitConfig", menuName = "UnitConfig")]
    public class UnitConfig : SerializedScriptableObject
    {
        [BoxGroup("General")]
        [field: SerializeField] public UnitVariant UnitVariant { get; private set; }
        
        [BoxGroup("ComponentsSettings")] 
        [field: OdinSerialize] public ComponentsSettingsHolder ComponentsSettingsHolder { get; private set; }= new();
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            ComponentsSettingsHolder.OnManualValidate();
        }

#endif
    }
}
