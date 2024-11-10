using BT.Shared;
using Interfaces;
using UnityEngine;
using Utility;

namespace Components.Combat.Weapons
{
    public class Weapon : MonoBehaviour
    {
        public WeaponStats WeaponStats => _weaponStats.Clone();
        [SerializeField] private WeaponStats _weaponStats;
        private DamageableTarget _currentTarget;
        
        public void SetCurrentTarget(DamageableTarget target)
        {
            _currentTarget = target;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform == _currentTarget.TargetTr)
            {
                Debug.LogWarning("yes");
            }
        }
    }
}
