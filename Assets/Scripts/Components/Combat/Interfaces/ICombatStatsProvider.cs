using Components.Combat.Weapons;

namespace Components.Combat.Interfaces
{
    public interface ICombatStatsProvider
    {
        public WeaponStats GetCurrentCombatStats();
    }
}
