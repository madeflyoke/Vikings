using System;
using System.Collections.Generic;
using System.Linq;
using Components.Combat.Weapons.Enums;
using Components.Combat.Weapons.Interfaces;
using Utility;

namespace Components.Combat.Weapons
{
    public class WeaponSet : IDisposable
    {
        public event Action<WeaponSet> CallOnWeaponSetActivated;

        public bool Active { get; private set; }
        public WeaponStats WeaponStats { get; private set; }
        private WeaponMode WeaponMode { get; set; }
        private WeaponType WeaponsType { get; set; }

        private readonly WeaponPair _weaponPair;
        private readonly WeaponsHolder _weaponsHolder;

        public WeaponSet(WeaponPair weaponPair, WeaponsHolder weaponsHolder)
        {
            _weaponsHolder = weaponsHolder;
            _weaponPair = weaponPair;
            WeaponMode = WeaponMode.DUAL;
            _weaponPair.Item2.SetHandParent(false);
            Initialize();
        }

        public WeaponSet(Weapon weapon, WeaponsHolder weaponsHolder)
        {
            _weaponsHolder = weaponsHolder;
            _weaponPair = new WeaponPair() {Item1 = weapon};
            WeaponMode = WeaponMode.SINGLE;
            Initialize();
        }
        
        public void CallOnActivate()
        {
            CallOnWeaponSetActivated?.Invoke(this);
        }

        private void Initialize()
        {
            _weaponsHolder.CurrentWeaponSetChanged += OnWeaponSetChanged;
            _weaponsHolder.WeaponSetsDisabled += OnWeaponSetDisabled;
            _weaponsHolder.CurrenTargetChanged += SetTarget;

            var data = _weaponPair.Item1.WeaponData;
            WeaponsType = data.Type;
            WeaponStats = _weaponPair.GetWeaponStats();
        }

        private void SetTarget(DamageableTarget target)
        {
            _weaponPair.ForEach(x => x.SetTarget(target));
        }

        private void OnWeaponSetDisabled()
        {
            Active = false;
            _weaponPair.SetWeaponsActive(false);
        }
        
        private void OnWeaponSetChanged(WeaponSet weaponSet)
        {
            if (weaponSet==this)
            {
                Active = true;
                _weaponPair.SetWeaponsActive(true);
            }
        }

        public List<T> GetWeaponAttackHandlers<T>() where T : IWeaponAttackHandler
        {
            var result = new List<T> {_weaponPair.Item1.GetAttackHandler<T>()};
            if (_weaponPair.Item2!=null)
            {
                result.Add(_weaponPair.Item2.GetAttackHandler<T>());
            }

            return result;
        }

        public void Dispose()
        {
            _weaponsHolder.CurrentWeaponSetChanged -= OnWeaponSetChanged;
            _weaponsHolder.CurrenTargetChanged -= SetTarget;
            _weaponsHolder.WeaponSetsDisabled -= OnWeaponSetDisabled;
        }
    }

    public class WeaponPair
    {
        public Weapon Item1;
        public Weapon Item2;

        public WeaponPair()
        {
            
        }

        public WeaponPair(List<Weapon> weapons)
        {
            Item1 = weapons[0];
            Item2 = weapons[1];
        }
        
        public WeaponStats GetWeaponStats()
        {
            if (Item2==null)
            {
                return Item1.WeaponData.Stats;
            }
            else
            {
                return new WeaponStats()
                {
                    AttackDamage = Item1.WeaponData.Stats.AttackDamage+Item2.WeaponData.Stats.AttackDamage,
                    AttackRange = Math.Min(Item1.WeaponData.Stats.AttackRange, Item2.WeaponData.Stats.AttackRange),
                    AttackSpeed = Math.Min(Item1.WeaponData.Stats.AttackSpeed, Item2.WeaponData.Stats.AttackSpeed)
                };
            }
        }

        public void SetWeaponsActive(bool value)
        {
            Item1.SetWeaponActive(value);
            if (Item2)
            {
                Item2.SetWeaponActive(value);
            }
        }

        public void ForEach(Action<Weapon> action)
        {
            action?.Invoke(Item1);
            if (Item2 != null)
            {
                action?.Invoke(Item2);
            }
        }
    }
}
