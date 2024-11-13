using System.Collections.Generic;
using System.Linq;
using Components.Combat.Weapons.Enums;
using UnityEngine;

namespace Components.Combat.Weapons
{
    [CreateAssetMenu(fileName = "WeaponsConfig", menuName = "Weapons/WeaponsConfig")]
    public class WeaponsConfig : ScriptableObject
    {
        [SerializeField] private List<WeaponData> WeaponDatas;

        public WeaponData GetWeaponData(WeaponType type)
        {
            return WeaponDatas.FirstOrDefault(x => x.Type == type);
        }
    }
}
