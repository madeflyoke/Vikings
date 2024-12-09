using Components.Combat.Weapons;

namespace Components.Combat.Interfaces
{
    public interface IWeaponStatsProvider
    {
        public WeaponStats GetCombatStats();
    }
}
