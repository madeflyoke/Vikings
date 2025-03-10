using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Builders.Utility;
using Components.Combat.Weapons;
using Factories.Interfaces;
using Factories.Units.SubFactories;
using Factories.Units.SubFactories.Attributes;
using Factories.Units.SubFactories.Base;
using Sirenix.OdinInspector;
using Units.Base;
using Units.Configs;
using Units.Enums;
using UnityEngine;
using Utility;

namespace Factories.Units
{
    public class UnitsAbstractFactory : SerializedMonoBehaviour, IFactory<UnitEntity>
    {
        [SerializeField] private UnitsConfigsContainer _unitsConfigsContainer;
        [SerializeField] private WeaponsConfig _weaponsConfig;
        [SerializeField] private TeamsConfig _teamsConfig;
        [SerializeField, ReadOnly] private Dictionary<UnitVariant, UnitSubFactory> _subFactoriesMap;

        private readonly UnitProductRequestData _unitProductRequestData = new ();
        private readonly Dictionary<UnitVariant, UnitEntity> _cachedUnits = new();
        
        public UnitsAbstractFactory SetProductRequestData(UnitVariant unitVariant, Team team,
            Vector3 position = default,
            Quaternion rotation = default, 
            Transform parent = null)
        {
            _unitProductRequestData.TargetUnitVariant = unitVariant;
            _unitProductRequestData.TargetUnitTeam = team;
        
            _unitProductRequestData.SpawnData.Position = position;
            _unitProductRequestData.SpawnData.Rotation = rotation;
            _unitProductRequestData.SpawnData.Parent = parent;
            return this;
        }

        public UnitEntity CreateProduct()
        {
            // if (_cachedUnits.ContainsKey(_unitProductRequestData.TargetUnitClass))
            // {
            //     return MakeCopy(_cachedUnits[_unitProductRequestData.TargetUnitClass], _unitProductRequestData.SpawnData);
            // }

            return CreateTargetUnit();
        }

        private UnitEntity CreateTargetUnit()
        {
            return _subFactoriesMap[_unitProductRequestData.TargetUnitVariant]
                .Initialize(_unitProductRequestData.SpawnData, _unitProductRequestData.TargetUnitTeam)
                .CreateProduct().InitializeEntity();
        }
        
        private UnitEntity MakeCopy(UnitEntity original, CustomTransformData spawnData) //TODO Check in the end
        {
            GameObjectComponentBuilder<UnitEntity> goBuilder = new GameObjectComponentBuilder<UnitEntity>();

            return goBuilder
                .SetPrefab(original)
                .SetPosition(spawnData.Position)
                .SetRotation(spawnData.Rotation)
                .SetParent(spawnData.Parent)
                .Build();
        }

        
        private class UnitProductRequestData
        {
            public UnitVariant TargetUnitVariant = UnitVariant.NONE;
            public Team TargetUnitTeam = Team.NONE;
            public readonly CustomTransformData SpawnData = new();
        }
        
#if UNITY_EDITOR

        [Button]
        private void SetupSubFactories()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
  
            GameObject subFactoriesParent = new GameObject();
            subFactoriesParent.name = "SubFactories";
            subFactoriesParent.transform.parent = transform;
            
            _subFactoriesMap = new();
            
            void AddSubFactory<TFactory>() where TFactory: UnitSubFactory
            {
                UnitVariantAttribute attribute = CustomAttributeExtensions.GetCustomAttribute<UnitVariantAttribute>(typeof(TFactory));

                if (attribute==null)
                {
                    Debug.LogError("Unit's factory doesnt have corresponding UnitVariant attribute!");
                    return;
                }

                _subFactoriesMap.Add(attribute.UnitVariant, subFactoriesParent.AddComponent<TFactory>()
                    .SetupFactoryData(_unitsConfigsContainer.GetConfig(attribute.UnitVariant), _weaponsConfig, _teamsConfig));
            }
            
            AddSubFactory<BarbarianUnitFactory>();
            AddSubFactory<ArcherUnitFactory>();
            AddSubFactory<DummyUnitFactory>();
        }

   
        private bool EDITOR_IsPlayingMode => Application.isPlaying;
        
        [Button, ShowIf("EDITOR_IsPlayingMode")]
        private void ValidateSubFactoriesUnits()
        {
            Debug.LogWarning($"Units validation start.");

            var parentName = "TEST_UNITS";
            var previousParent = GameObject.Find(parentName);
            if (previousParent!=null)
            {
                Destroy(previousParent);
            }
            
            GameObject parent = new GameObject();
            parent.name = parentName;
            parent.tag = "EditorOnly";
            
            foreach (var item in Enum.GetValues(typeof(UnitVariant)))
            {
                var unitVariant = (UnitVariant) item;

                if (unitVariant!=UnitVariant.NONE)
                {
                    try
                    {
                        _subFactoriesMap[unitVariant]
                            .Initialize(new CustomTransformData()
                            {
                                Parent = parent.transform,
                                Position = Vector3.zero + new Vector3((int) unitVariant*3,0,0) 
                            }, Team.ALLIES)
                            .CreateProduct();
                    }
                    catch(Exception e)
                    {
                        DestroyImmediate(parent);
                        Debug.LogWarning($"<color=red>Aborting validation of {unitVariant} unit due to error!</color>");
                        ExceptionDispatchInfo.Capture(e).Throw();
                        return;
                    }
                    Debug.LogWarning($"<color=green>Validation of {unitVariant} unit completed!</color>");
                }
            }
        }
        
#endif
    }
}
