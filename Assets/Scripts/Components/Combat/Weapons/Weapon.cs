using System;
using System.Collections.Generic;
using System.Linq;
using Components.Combat.Weapons.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;

namespace Components.Combat.Weapons
{
    public class Weapon : SerializedMonoBehaviour
    {
        public WeaponStats WeaponStats => _weaponStats.Clone();
        [SerializeField] private WeaponStats _weaponStats;
        
        [SerializeField] private List<IWeaponAttackHandler> _weaponActionHandlers;

        public void Initialize()
        {
            _weaponActionHandlers.ForEach(x=>x.Initialize());
        }

        public void SetCurrentTarget(DamageableTarget target)
        {
            _weaponActionHandlers.ForEach(x=>x.SetTarget(target));
        }

        public T GetWeaponActionHandler<T>() where T: IWeaponAttackHandler
        {
            return (T)_weaponActionHandlers.FirstOrDefault(x => x.GetType() == typeof(T));
        }
    }
}
