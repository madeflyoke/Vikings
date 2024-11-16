using System;
using System.Collections.Generic;
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
        
        [SerializeField] private List<Collider> _hitColliders;
        
        public DamageableTarget CurrentTarget { get; private set; }
        
        private IHitReceiver _hitReceiver;
        private CompositeDisposable _collidersDisposable;
        private Weapon _relatedWeapon;

        public void Initialize(Weapon relatedWeapon)
        {
            _relatedWeapon = relatedWeapon;
            
            _collidersDisposable = new CompositeDisposable();
            foreach (var col in _hitColliders)
            {
                col.OnTriggerEnterAsObservable().Where(_=>_relatedWeapon.Activated).Subscribe(ManualOnTriggerEnter).AddTo(_collidersDisposable);
            }
        }
        
        public void SetTarget(DamageableTarget damageableTarget)
        {
            CurrentTarget = damageableTarget;
            _hitReceiver = damageableTarget.Damageable.HitReceiver;
        }
        
        public void SetColliderActive(bool value)
        {
            _hitColliders.ForEach(x=>x.enabled=value);
        }
        
        private void ManualOnTriggerEnter(Collider other)
        {
            if (other == _hitReceiver.OverallCollider)
            {
                HitEvent?.Invoke(CurrentTarget);
            }
        }
        
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            SetColliderActive(false);
            _hitColliders.ForEach(x=>x.isTrigger = true);
        }

#endif

        public void Dispose()
        {
            _collidersDisposable?.Dispose();
        }
    }
}
