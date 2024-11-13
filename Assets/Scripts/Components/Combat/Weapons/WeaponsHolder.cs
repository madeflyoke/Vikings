using System;
using System.Collections.Generic;
using System.Linq;
using Components.Combat.Actions.Conditions;
using Components.Combat.Weapons.Enums;
using Components.Combat.Weapons.Interfaces;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using Utility;

namespace Components.Combat.Weapons
{
    public class WeaponsHolder : IDisposable
    {
        public event Action CurrentWeaponSetChanged;

        public WeaponStats CurrentWeaponStats => _currentWeaponSet.WeaponStats.Clone();
        private WeaponSet _currentWeaponSet;
        
        private readonly List<Weapon> _allWeapons;
        private readonly List<WeaponSet> _weaponsSets;

        public WeaponsHolder(List<Weapon> allWeapons, List<WeaponSet> weaponsSets)
        {
            _allWeapons = allWeapons;
            _weaponsSets = weaponsSets;
            _weaponsSets.ForEach(x => x.WeaponSetActivated += OnWeaponSetActivated);
        }

        private void OnWeaponSetActivated(WeaponSet weaponSet)
        {
            _currentWeaponSet = weaponSet;
            CurrentWeaponSetChanged?.Invoke();
        }

        public void Initialize()
        {
            _allWeapons.ForEach(x=>x.Initialize());
            OnWeaponSetActivated(_weaponsSets[0]); //default weapon
        }
        
        public void SetTarget(DamageableTarget target)
        {
            _weaponsSets.ForEach(x=>x.SetTarget(target));
        }
        
        public void Dispose()
        {
            _weaponsSets.ForEach(x => x.WeaponSetActivated -= OnWeaponSetActivated);
        }
    }
}
