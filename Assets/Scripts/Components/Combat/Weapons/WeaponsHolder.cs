using System;
using System.Collections.Generic;
using Components.BT.Actions.Conditions;
using Components.Combat.Interfaces;
using Factories.Interfaces;
using Utility;

namespace Components.Combat.Weapons
{
    public class WeaponsHolder : IDisposable, IFactoryProduct, IWeaponStatsProvider
    {
        public event Action WeaponSetsDisabled;
        public event Action<WeaponSet> CurrentWeaponSetChanged;
        public event Action<DamageableTarget> CurrenTargetChanged;

        private WeaponStats currentWeaponStats => _currentWeaponSet.WeaponStats;
        
        private WeaponSet _currentWeaponSet;
        
        private readonly List<Weapon> _allWeapons;
        private readonly List<WeaponSet> _weaponsSets;

        public WeaponsHolder(List<Weapon> allWeapons, List<WeaponSet> weaponsSets)
        {
            _allWeapons = allWeapons;
            _weaponsSets = weaponsSets; //empty lists in constructor stage, do everything in Initialize
        }
        
        public void Initialize()
        {
            _weaponsSets.ForEach(x => x.CallOnWeaponSetActivated += OnWeaponSetActivated);
            _allWeapons.ForEach(x=>x.Initialize());
            OnWeaponSetActivated(_weaponsSets[0]); //default weapon
        }
        
        public void SetTarget(DamageableTarget target)
        {
            CurrenTargetChanged?.Invoke(target);
        }
        
        private void OnWeaponSetActivated(WeaponSet weaponSet)
        {
            WeaponSetsDisabled?.Invoke();
            _currentWeaponSet = weaponSet;
            CurrentWeaponSetChanged?.Invoke(_currentWeaponSet);
        }

        public WeaponSet GetWeaponSetByConditions(CombatActionConditions combatActionConditions)
        {
            return _weaponsSets.Find(set => set.ValidateWeaponSetByConditions(combatActionConditions));
        }
        
        public void Dispose()
        {
            _weaponsSets.ForEach(x => x.CallOnWeaponSetActivated -= OnWeaponSetActivated);
        }

        public WeaponStats GetCombatStats()
        {
            return currentWeaponStats;
        }
    }
}
