using Components.Combat.Weapons;

namespace Components.Combat.Interfaces
{
    public interface ICombatStatsCopyProvider
    {
        public WeaponStats GetCombatStatsCopy();
    }
}
