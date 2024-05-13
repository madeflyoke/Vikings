using System;
using System.Collections.Generic;
using System.Linq;
using Components.Settings.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Components.Settings
{
    [Serializable]
    public class ComponentsSettingsHolder
    {
        [field: SerializeField,OnCollectionChanged(nameof(AfterComponentsSettingsChanged))] 
        public List<IComponentSettings> ComponentsSettings { get; private set; }
        
        [Button]
        public void TryAddComponentSettings<TComponentType> () where TComponentType: IComponentSettings
        {
            ComponentsSettings ??= new List<IComponentSettings>();
            if (IsComponentSettingsExists<TComponentType>()==false)
            {
                ComponentsSettings.Add(Activator.CreateInstance<TComponentType>());
            }
        }
        
        public TComponentType GetComponentSettings<TComponentType>() where TComponentType : IComponentSettings
        {
            if (IsComponentSettingsExists<TComponentType>(out IComponentSettings statsSettings))
            {
                return (TComponentType) statsSettings;
            }

            throw new Exception(
                $"Component settings holder does NOT have required component {typeof(TComponentType).Name}!");
        }
        
        private bool IsComponentSettingsExists<TComponentType>() where TComponentType : IComponentSettings
        {
            return ComponentsSettings.FirstOrDefault(x => x.GetType() == typeof(TComponentType)) != null;
        }

        private bool IsComponentSettingsExists<TComponentType>(out IComponentSettings statsSettings) where TComponentType : IComponentSettings
        {
            statsSettings = ComponentsSettings.FirstOrDefault(x => x.GetType() == typeof(TComponentType));
            return statsSettings != null;
        }

#if UNITY_EDITOR
        
        private void AfterComponentsSettingsChanged(CollectionChangeInfo info)
        {
            var component = (IComponentSettings) info.Value;
            
            if (info.ChangeType==CollectionChangeType.Add && ComponentsSettings.Count(x => x.GetType()==component.GetType())>1)
            {
                ComponentsSettings.Remove(component);
            }
        }
        
#endif
    }
}
