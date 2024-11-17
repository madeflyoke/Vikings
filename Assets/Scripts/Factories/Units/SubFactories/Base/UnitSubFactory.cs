using System;
using System.Text;
using Components;
using Components.Combat.Weapons;
using Components.Interfaces;
using Components.TagHolder;
using Factories.Components;
using Factories.Decorators;
using Factories.Interfaces;
using Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Units.Base;
using Units.Configs;
using Units.Enums;
using UnityEngine;
using Utility;

namespace Factories.Units.SubFactories.Base
{
    public abstract class UnitSubFactory : MonoBehaviour, IFactory<UnitEntity>
    {
        protected IReadOnlyEntity Entity;

        [SerializeField, ReadOnly] protected UnitConfig Config;
        [SerializeField, ReadOnly] protected WeaponsConfig WeaponsConfig;
        [SerializeField, ReadOnly] protected TeamsConfig TeamsConfig;
        [SerializeField, ReadOnly] private EntityHolder _entityHolder;
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
        
        public UnitSubFactory SetupFactoryData(UnitConfig config, WeaponsConfig weaponsConfig, TeamsConfig teamsConfig)
        {
            Config = config;
            WeaponsConfig = weaponsConfig;
            TeamsConfig = teamsConfig;
            _entityHolder = Resources.Load<EntityHolder>(ResourcesPaths.BaseEntities.EntityHolderPath);

            return this;
        }
#endif
    }
}
