using System;

namespace Components.Combat.Weapons
{
    [Serializable]
    public class WeaponStats
    {
        public int AttackDamage;
        public float AttackSpeed;
        public float AttackRange;

        public WeaponStats Clone()
        {
            return new WeaponStats()
            {
                AttackDamage = this.AttackDamage,
                AttackSpeed = this.AttackSpeed,
                AttackRange = this.AttackRange
            };
        }
    }
}
