using System;
using System.Text;
using Components;
using Components.Interfaces;
using Components.TagHolder;
using Factories.Components;
using Factories.Decorators;
using Factories.Interfaces;
using Interfaces;
using Units.Base;
using Units.Configs;
using Units.Enums;
using Unity.Collections;
using UnityEngine;
using Utility;

namespace Factories.Units.SubFactories.Base
{
    public abstract class UnitSubFactory : MonoBehaviour, IFactory<UnitEntity>
    {
        protected IReadOnlyEntity Entity;
        
        [field: SerializeField, ReadOnly] protected UnitConfig Config { get; private set; }
        [SerializeField] private EntityHolder _entityHolder;
        private UnitEntity _unitEntity;

        public UnitSubFactory Initialize(CustomTransformData spawnData, Team team)
        {
            var uniqueId = Guid.NewGuid().ToString().Substring(0, 5);
            var unitName = new StringBuilder().Append(Config.UnitVariant).Append("_").Append(uniqueId).ToString();
            
            _unitEntity = new EntityFactory<UnitEntity>(spawnData, _entityHolder, unitName).CreateProduct();
            DecorateBy(new TagHolderDecorator(new UnitTagHolder(team)));
            
            Entity = _unitEntity;
            
#if UNITY_EDITOR
            Debug.LogWarning($"Unit {Config.UnitVariant} initialized with base entity.");
#endif
            return this;
        }

        public virtual UnitEntity CreateProduct()
        {
            return _unitEntity;
        }

        protected IEntityComponent DecorateBy(IEntityDecorator decorator)
        {
            var component = decorator.Decorate();
            _unitEntity.AddEntityComponent(component);
            
#if UNITY_EDITOR
            Debug.LogWarning($"Unit {Config.UnitVariant} decorated with {decorator.GetType().Name}.");
#endif
            
            return component;
        }

#if UNITY_EDITOR
        
        public UnitSubFactory SetRelatedConfig(UnitConfig config)
        {
            Config = config;
            _entityHolder = Resources.Load<EntityHolder>("EmptyUnitHolder");

            return this;
        }
#endif
    }
}
