using System;
using System.Collections.Generic;
using System.Linq;
using Components.Combat.Actions;
using Components.Combat.Interfaces;
using Components.Combat.Weapons;
using Components.Interfaces;
using Utility;

namespace Components.Combat
{
    public class CombatComponent : IEntityComponent, ICombatStatsProvider, ICombatTargetHolder
    {
        public event Action<float> AttackSpeedSetEvent;
        
        public List<CombatAction> CombatActions { get; }
        
        private WeaponStats BaseCombatStats=>_weaponsHolder.CurrentWeaponStats;
        private WeaponStats _currentCombatStats;

        private readonly WeaponsHolder _weaponsHolder;

        public CombatComponent(WeaponsHolder weaponsHolder, List<CombatAction> actions)
        {
            CombatActions = actions;
            _weaponsHolder = weaponsHolder;
            _weaponsHolder.CurrentWeaponSetChanged += OnWeaponSetChanged;
            
            CombatActions.ForEach(x=>x.SetCombatStatsProvider(this));
        }

        private void OnWeaponSetChanged()
        {
            _currentCombatStats = BaseCombatStats;
        }

        public void InitializeComponent()
        {
            _weaponsHolder.Initialize();
            SetAttackSpeed();
        }

        public void SetAttackSpeed(float multiplier = 1f)
        {
            _currentCombatStats.AttackSpeed = BaseCombatStats.AttackSpeed * multiplier;
            AttackSpeedSetEvent?.Invoke(_currentCombatStats.AttackSpeed);
        }

        public WeaponStats GetCurrentCombatStats()
        {
            return _currentCombatStats;
        }

        public void SetCombatTarget(DamageableTarget combatTarget)
        {
            _weaponsHolder.SetTarget(combatTarget);
        }
     
        public void Dispose()
        {
            _weaponsHolder.CurrentWeaponSetChanged -= OnWeaponSetChanged;
        }
    }
}
