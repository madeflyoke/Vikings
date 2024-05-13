using System;
using System.Collections.Generic;
using Components.Interfaces;
using Factories.Interfaces;
using Interfaces;
using Sirenix.OdinInspector;
using Units.Enums;
using UnityEngine;

namespace Units.Base
{
    public class UnitEntity : MonoBehaviour, IEntity, IReadOnlyEntity, IFactoryProduct
    {
        [ShowInInspector, ReadOnly] private readonly Dictionary<Type, IEntityComponent> _components = new();

        public IEntity AddEntityComponent (IEntityComponent unitEntityComponent)
        {
            var componentType = unitEntityComponent.GetType();
            if (_components.ContainsKey(componentType)==false)
            {
                _components.Add(componentType, unitEntityComponent);
            }

            return this;
        }
        
        public TComponent GetEntityComponent<TComponent>() where TComponent : IEntityComponent
        {
            if (_components.ContainsKey(typeof(TComponent)))
            {
                return (TComponent) _components[typeof(TComponent)];
            }
            
            throw new Exception($"No component with type: {typeof(TComponent)}!");
        }
    }
}
