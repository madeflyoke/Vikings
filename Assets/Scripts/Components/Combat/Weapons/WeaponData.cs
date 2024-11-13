using System;
using Components.Combat.Weapons.Enums;
using UnityEngine;

namespace Components.Combat.Weapons
{
    [Serializable]
    public class WeaponData
    {
        public WeaponType Type;
        public Weapon Prefab;
        public WeaponStats Stats => _stats.Clone();
        [SerializeField] private WeaponStats _stats;
    }
}
