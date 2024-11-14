using System;
using System.Collections.Generic;
using System.Linq;
using Components.Animation;
using Components.Animation.Interfaces;
using Components.Combat.Actions;
using Components.Combat.Interfaces;
using Components.Combat.Weapons;
using Components.Interfaces;
using Utility;

namespace Components.Combat
{
    public class CombatComponent : IEntityComponent, ICombatStatsProvider, ICombatTargetHolder, IAnimationCallerHolder
    {
        public List<CombatAction> CombatActions { get; }
        
        public AnimationCaller AnimationCaller { get; }
        private WeaponStats BaseCombatStats=>_weaponsHolder.CurrentWeaponStats;
        private WeaponStats _currentCombatStats;

        private readonly WeaponsHolder _weaponsHolder;

        public CombatComponent(WeaponsHolder weaponsHolder, List<CombatAction> actions)
        {
            CombatActions = actions;
            _weaponsHolder = weaponsHolder;
            _weaponsHolder.CurrentWeaponSetChanged += OnWeaponSetChanged;
            AnimationCaller = new AnimationCaller();
            
            CombatActions.ForEach(x=>x.SetCombatStatsProvider(this));
        }

        private void OnWeaponSetChanged(WeaponSet weaponSet)
        {
            _currentCombatStats = BaseCombatStats;
        }

        public void InitializeComponent()
        {
            _weaponsHolder.Initialize();
            SetAttackSpeed(.3f);
        }

        public void SetAttackSpeed(float multiplier = 1f)
        {
            _currentCombatStats.AttackSpeed = BaseCombatStats.AttackSpeed * multiplier;
            AnimationCaller.CallOnParameterValueChange?.Invoke(AnimatorParametersNames.CombatActionSpeedMultiplier, _currentCombatStats.AttackSpeed);
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
