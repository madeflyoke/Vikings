using System;
using Components.Settings.Interfaces;
using Sirenix.OdinInspector;
using UnityEditor.Presets;
using UnityEngine;

namespace Components.Settings
{
    [Serializable]
    public class MovementComponentSettings : IComponentSettings
    {
        public Preset NavMeshAgentTemplate => _navMeshAgentTemplate;
        
        [SerializeField, OnValueChanged(nameof(ValidatePreset))] private Preset _navMeshAgentTemplate;

        
#if UNITY_EDITOR
        
        private void ValidatePreset()
        {
            if (_navMeshAgentTemplate!=null&&_navMeshAgentTemplate.GetTargetTypeName().Contains("NavMeshAgent",StringComparison.OrdinalIgnoreCase)==false)
            {
                Debug.LogWarning("Incorrect Preset Type!");
                _navMeshAgentTemplate = null;
            }
        }
        public void OnManualValidate(){}
        
#endif

    }
}
