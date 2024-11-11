using System;
using System.Threading;
using Components.Combat.Interfaces;
using Components.Combat.Weapons.Interfaces;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utility;

namespace Components.Combat.Weapons.Handlers
{
    [Serializable]
    public class MeleeAttackWeaponHandler : IWeaponAttackHandler, IDisposable
    {
        public event Action<DamageableTarget> HitEvent;
        
        [SerializeField] private Collider _hitCollider;
        
        public DamageableTarget CurrentTarget { get; private set; }
        private IHitReceiver _hitMarker;
        private IDisposable _colliderDis;

        public void Initialize()
        {
            _colliderDis = _hitCollider.OnTriggerEnterAsObservable().Subscribe(ManualOnTriggerEnter);
        }
        
        public void SetTarget(DamageableTarget damageableTarget)
        {
            CurrentTarget = damageableTarget;
            _hitMarker = damageableTarget.TargetTr.GetComponentInChildren<IHitReceiver>();
        }
        
        public void SetColliderActive(bool value)
        {
            _hitCollider.enabled = value;
        }
        
        private void ManualOnTriggerEnter(Collider other)
        {
            if (other == _hitMarker.HitCollider)
            {
                HitEvent?.Invoke(CurrentTarget);
            }
        }
        
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            SetColliderActive(false);
        }

#endif

        public void Dispose()
        {
            _colliderDis?.Dispose();
        }
    }
}
