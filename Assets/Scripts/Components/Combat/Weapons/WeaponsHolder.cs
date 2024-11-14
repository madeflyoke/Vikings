using System;
using System.Collections.Generic;
using Utility;

namespace Components.Combat.Weapons
{
    public class WeaponsHolder : IDisposable
    {
        public event Action WeaponSetsDisabled;
        public event Action<WeaponSet> CurrentWeaponSetChanged;
        public event Action<DamageableTarget> CurrenTargetChanged;

        public WeaponStats CurrentWeaponStats => _currentWeaponSet.WeaponStats.Clone();
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
        
        public void Dispose()
        {
            _weaponsSets.ForEach(x => x.CallOnWeaponSetActivated -= OnWeaponSetActivated);
        }
    }
}
