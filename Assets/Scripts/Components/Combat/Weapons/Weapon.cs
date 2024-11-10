using System;
using BT.Shared;
using Components.Combat.Interfaces;
using Components.View;
using Interfaces;
using UnityEngine;
using Utility;

namespace Components.Combat.Weapons
{
    public class Weapon : MonoBehaviour
    {
        public event Action<DamageableTarget> HitEvent;
        
        public WeaponStats WeaponStats => _weaponStats.Clone();
        [SerializeField] private WeaponStats _weaponStats;
        [SerializeField] private Collider _hitCollider;
        private IHitReceiver _hitMarker;
        private DamageableTarget _currentTarget;

        public void SetCurrentTarget(DamageableTarget target)
        {
            _currentTarget = target;
            _hitMarker = _currentTarget.TargetTr.GetComponentInChildren<IHitReceiver>();
        }

        public void SetColliderActive(bool value)
        {
            _hitCollider.enabled = value;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other == _hitMarker.HitCollider)
            {
                HitEvent?.Invoke(_currentTarget);
            }
        }
        
        #if UNITY_EDITOR

        private void OnValidate()
        {
            SetColliderActive(false);
        }

#endif
    }
}
