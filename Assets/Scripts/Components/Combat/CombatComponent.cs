using System;
using System.Collections.Generic;
using Components.Combat.Actions;
using Components.Combat.Interfaces;
using Components.Combat.Weapons;
using Components.Interfaces;
using Utility;

namespace Components.Combat
{
    public class CombatComponent : IEntityComponent, ICombatStatsCopyProvider, ICombatTargetHolder
    {
        public event Action<float> IncreaseAttackSpeedEvent;
        
        public List<CombatAction> CombatActions { get; }

        private DamageableTarget _currentTarget;
        private readonly Weapon _currentWeapon;
        
        private readonly WeaponStats _baseCombatStats;
        private readonly WeaponStats _currentCombatStats;

        public CombatComponent(Weapon weapon, List<CombatAction> actions)
        {
            CombatActions = actions;
           
            _currentWeapon = weapon;
            
            _baseCombatStats = weapon.WeaponStats;
            _currentCombatStats = weapon.WeaponStats;
            
            CombatActions.ForEach(x=>x.SetCombatStatsProvider(this));
        }

        public void InitializeComponent()
        {
            SetAttackSpeed();
        }

        public void SetAttackSpeed(float multiplier = 1f)
        {
            _currentCombatStats.AttackSpeed = _baseCombatStats.AttackSpeed * multiplier;
            IncreaseAttackSpeedEvent?.Invoke(_currentCombatStats.AttackSpeed);
        }

        public WeaponStats GetCombatStatsCopy()
        {
            return _baseCombatStats.Clone();
        }

        public void SetCombatTarget(DamageableTarget combatTarget)
        {
            _currentTarget = combatTarget;
            _currentWeapon.SetCurrentTarget(_currentTarget);
        }
    }
}
