using System.Collections.Generic;
using Components.Animation.Interfaces;
using Components.BT.Actions.Interfaces;
using Components.Combat.Interfaces;
using Components.Combat.Weapons;
using Components.Interfaces;
using Utility;

namespace Components.Combat
{
    public class CombatComponent : IEntityComponent, IWeaponStatsProvider, ICombatTargetHolder
    {
        public List<IBehaviorAction> CombatActions { get; }
        
        private WeaponStats BaseCombatStats=>_weaponsHolder.GetCombatStats().Copy();
        private WeaponStats _currentCombatStats;

        private readonly IAnimatorValueChanger _animatorValueChanger;
        private readonly WeaponsHolder _weaponsHolder;

        public CombatComponent(WeaponsHolder weaponsHolder, List<IBehaviorAction> actions, IAnimatorValueChanger animatorValueChanger)
        {
            CombatActions = actions;
            _weaponsHolder = weaponsHolder;
            _weaponsHolder.CurrentWeaponSetChanged += OnWeaponSetChanged;
            _animatorValueChanger = animatorValueChanger;
        }

        private void OnWeaponSetChanged(WeaponSet weaponSet)
        {
            _currentCombatStats = BaseCombatStats;
            SetAttackSpeed();
        }

        public void InitializeComponent()
        {
            _weaponsHolder.Initialize();
        }

        public void SetAttackSpeed(float multiplier = 1f)
        {
            _currentCombatStats.AttackSpeed = BaseCombatStats.AttackSpeed * multiplier;
            _animatorValueChanger.SetParameterValue(AnimatorParametersNames.CombatActionSpeedMultiplier, _currentCombatStats.AttackSpeed);
        }

        public WeaponStats GetCombatStats()
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
