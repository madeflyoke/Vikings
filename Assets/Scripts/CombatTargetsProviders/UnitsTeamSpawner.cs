using System;
using System.Collections.Generic;
using System.Linq;
using CombatTargetsProviders.Interfaces;
using Components;
using Components.Combat.Interfaces;
using Components.Health;
using Factories.Units;
using Interfaces;
using Units.Base;
using Units.Enums;
using UnityEngine;
using Utility;

namespace CombatTargetsProviders
{
    [Serializable]
    public class UnitsTeamSpawner : ICombatTargetsProvider
    {
        [field: SerializeField] public Team Team { get; private set; }
        
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private List<UnitVariant> _unitVariants;

        private List<UnitEntity> _spawnedUnits;
        private UnitsAbstractFactory _unitsAbstractFactory;

        private List<DamageableTarget> _aliveTargets;
        
        public UnitsTeamSpawner(Team team)
        {
            Team = team;
            _spawnedUnits = new();
        }
        
        public void Initialize(UnitsAbstractFactory unitsAbstractFactory)
        {
            _unitsAbstractFactory = unitsAbstractFactory;
        }
        
        public void Spawn()
        {
            if (_spawnPoints.Count<_unitVariants.Count || _spawnPoints.Count==0)
            {
                return;
            }

            _spawnedUnits ??= new();
            
            for (int i = 0; i < _spawnPoints.Count; i++)
            {
                if (_unitVariants.Count-1>=i)
                {
                    var pointTr = _spawnPoints[i];
                    var unitClass = _unitVariants[i];

                    _spawnedUnits.Add(_unitsAbstractFactory
                        .SetProductRequestData(unitClass, Team, pointTr.position, pointTr.rotation, pointTr)
                        .CreateProduct());
                }
            }

            SetupAliveTargets();
        }
        
        public List<DamageableTarget> GetAliveCombatTargets()
        {
            return _aliveTargets.ToList();
        }
        
        private void SetupAliveTargets()
        {
            _aliveTargets = new List<DamageableTarget>();
            _spawnedUnits.ForEach(x=>
            {
                var damageable = x.GetEntityComponent<HealthComponent>();
                damageable.DeadEvent += OnUnitDie;
                _aliveTargets.Add(new DamageableTarget()
                {
                    Damageable = damageable,
                    TargetTr = x.GetEntityComponent<EntityHolder>().SelfTransform
                });
            });
        }

        private void OnUnitDie(IDamageable damageable)
        {
            damageable.DeadEvent -= OnUnitDie;
            var target = _aliveTargets.FirstOrDefault(x => x.Damageable == damageable);
            if (_aliveTargets.Contains(target))
            {
                _aliveTargets.Remove(target);
            }
        }
    }
}
