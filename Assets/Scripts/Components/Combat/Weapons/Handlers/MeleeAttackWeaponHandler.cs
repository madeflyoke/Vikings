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
        private bool _activated;

        public void Initialize()
        {
            _collidersDisposable = new CompositeDisposable();
            foreach (var col in _hitColliders)
            {
                col.OnTriggerEnterAsObservable().Where(_=>_activated).Subscribe(ManualOnTriggerEnter).AddTo(_collidersDisposable);
            }
        }
        
        public void OnWeaponStateChanged(bool isActive)
        {
            _activated = isActive;
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
        
        public void Dispose()
        {
            _collidersDisposable?.Dispose();
        }
        
#if UNITY_EDITOR

        public void EDITOR_ManualValidate()
        {
            SetColliderActive(false);
            _hitColliders.ForEach(x=>x.isTrigger = true);
        }

#endif
    }
}
