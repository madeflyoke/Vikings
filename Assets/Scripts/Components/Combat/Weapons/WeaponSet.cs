using System;
using System.Collections.Generic;
using Components.BT.Actions.Conditions;
using Components.Combat.Weapons.Enums;
using Components.Combat.Weapons.Interfaces;
using Utility;

namespace Components.Combat.Weapons
{
    public class WeaponSet : IDisposable
    {
        public event Action<WeaponSet> CallOnWeaponSetActivated;

        private bool Active { get; set; }
        public WeaponStats WeaponStats { get; private set; }
        
        private WeaponMode _weaponMode;
        private WeaponType _weaponsType;

        private readonly WeaponPair _weaponPair;
        private readonly WeaponsHolder _weaponsHolder;

        public WeaponSet(WeaponPair weaponPair, WeaponsHolder weaponsHolder)
        {
            _weaponsHolder = weaponsHolder;
            _weaponPair = weaponPair;
            _weaponMode = WeaponMode.DUAL;
            _weaponPair.Item2.SetHandParent(false);
            Initialize();
        }

        public WeaponSet(Weapon weapon, WeaponsHolder weaponsHolder)
        {
            _weaponsHolder = weaponsHolder;
            _weaponPair = new WeaponPair() {Item1 = weapon};
            _weaponMode = WeaponMode.SINGLE;
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

            _weaponsType = _weaponPair.Item1.WeaponType;
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

        public bool ValidateWeaponSetByConditions(CombatActionConditions conditions)
        {
            if (conditions.WeaponType!=_weaponsType || conditions.WeaponMode!=_weaponMode)
            {
                return false;
            }

            if (_weaponPair.Item1.ValidateWeaponByConditions(conditions))
            {
                if (conditions.WeaponMode==WeaponMode.SINGLE)
                {
                    return true;
                }
                else if (_weaponPair.Item2!=null && _weaponPair.Item2.ValidateWeaponByConditions(conditions))
                {
                    return true;
                }
            }

            return false;
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
                return Item1.GetCombatStats();
            }
            else
            {
                return new WeaponStats()
                {
                    AttackDamage = Item1.GetCombatStats().AttackDamage+Item2.GetCombatStats().AttackDamage,
                    AttackRange = Math.Min(Item1.GetCombatStats().AttackRange, Item2.GetCombatStats().AttackRange),
                    AttackSpeed = Math.Min(Item1.GetCombatStats().AttackSpeed, Item2.GetCombatStats().AttackSpeed)
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
