using System;

namespace Components.Combat.Weapons
{
    [Serializable]
    public class WeaponStats
    {
        public float AttackDamage;
        public float AttackSpeed;
        public float AttackRange;

        public WeaponStats Copy()
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
