using UnityEngine;
using Utility;

namespace Components.Combat.Weapons.Interfaces
{
    public interface IWeaponAttackHandler
    {
        public DamageableTarget CurrentTarget { get; }
        public void Initialize();

        public void SetTarget(DamageableTarget damageableTarget);
        public void OnWeaponStateChanged(bool isActive);

        #if UNITY_EDITOR
        public void EDITOR_ManualValidate();
        
        #endif
    }
}
