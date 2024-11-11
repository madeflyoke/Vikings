using UnityEngine;
using Utility;

namespace Components.Combat.Weapons.Interfaces
{
    public interface IWeaponAttackHandler
    {
        public DamageableTarget CurrentTarget { get; }

        public void SetTarget(DamageableTarget damageableTarget);
        public void Initialize();
    }
}
