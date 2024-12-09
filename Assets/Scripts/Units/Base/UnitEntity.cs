using System;
using System.Collections.Generic;
using Components.Combat;
using Components.Interfaces;
using Components.TagHolder;
using Factories.Interfaces;
using Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Units.Enums;
using UnityEngine;

namespace Units.Base
{
    public class UnitEntity : MonoBehaviour, IEntity, IReadOnlyEntity, IFactoryProduct
    {
        [ShowInInspector, ReadOnly] private readonly Dictionary<Type, IEntityComponent> _components = new();

        public UnitEntity InitializeEntity()
        {
            _components.Values.ForEach(x => x.InitializeComponent());
            return this;
        }
        
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

#if UNITY_EDITOR

        private Team EDITOR_team = Team.NONE;
        
        private void OnDrawGizmos()
        {
            if (EDITOR_team==Team.NONE)
            {
                EDITOR_team = GetEntityComponent<UnitTagHolder>().Team;
            }

            if (EDITOR_team==Team.ENEMIES)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.blue;
            }
            Gizmos.DrawSphere(transform.position +Vector3.up*3f, 0.2f);
        }

#endif
    }
}
