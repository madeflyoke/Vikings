using System;
using System.Collections.Generic;
using System.Linq;
using Components.Combat.Weapons.Enums;
using Components.Combat.Weapons.Interfaces;
using Utility;

namespace Components.Combat.Weapons
{
    public class WeaponSet
    {
        public event Action<WeaponSet> WeaponSetActivated; 
        
        public WeaponStats WeaponStats { get; private set; }
        public WeaponMode WeaponMode { get; private set; }
        public WeaponType WeaponsType { get; private set; }
        
        private readonly WeaponPair _weaponPair;
        
        public WeaponSet(WeaponPair weaponPair)
        {
            _weaponPair = weaponPair;
            WeaponMode = WeaponMode.DUAL;
            _weaponPair.Item2.SwitchHandParent();
            SetWeaponSetData();
        }

        public WeaponSet(Weapon weapon)
        {
            _weaponPair = new WeaponPair(){Item1 = weapon};
            WeaponMode = WeaponMode.SINGLE;
            SetWeaponSetData();
        }

        private void SetWeaponSetData()
        {
            var data = _weaponPair.Item1.WeaponData;
            WeaponsType = data.Type;
            WeaponStats = _weaponPair.GetWeaponStats();
        }

        public void SetTarget(DamageableTarget target)
        {
            _weaponPair.ForEach(x=>x.SetTarget(target));
        }
        
        public void SetActive(bool value)
        {
            if (value)
            {
                WeaponSetActivated?.Invoke(this);
            }
            _weaponPair.ForEach(x=>x.ActivateWeapon(value));
        }
        
        public List<T> GetWeaponAttackHandlers<T>() where T: IWeaponAttackHandler
        {
            return new List<T>()
            {
                _weaponPair.Item1.GetAttackHandler<T>(),
                _weaponPair.Item2.GetAttackHandler<T>()
            };
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

        public void ForEach(Action<Weapon> action)
        {
            action?.Invoke(Item1);
            action?.Invoke(Item2);
        }
    }
}
