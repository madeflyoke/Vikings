using System;

namespace Components.Combat
{
    [Serializable]
    public class CommonCombatStats
    {
        public int AttackDamage;
        public float AttackSpeed;
        public float AttackRange;

        public CommonCombatStats Clone()
        {
            return new CommonCombatStats()
            {
                AttackDamage = this.AttackDamage,
                AttackSpeed = this.AttackSpeed,
                AttackRange = this.AttackRange
            };
        }
    }
}
