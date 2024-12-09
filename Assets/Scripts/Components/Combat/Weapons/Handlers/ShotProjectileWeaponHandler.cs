using System;
using Components.Combat.Interfaces;
using Components.Combat.Projectiles;
using Components.Combat.Weapons.Interfaces;
using UnityEngine;
using Utility;
using Object = UnityEngine.Object;

namespace Components.Combat.Weapons.Handlers
{
    [Serializable]
    public class ShotProjectileWeaponHandler : IWeaponAttackHandler, IDisposable
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Transform _projectileSpawnPoint;
        public DamageableTarget CurrentTarget { get; private set; }
        private IHitReceiver _hitReceiver;
        
        public void Initialize()
        {
        }
        
        public void SetTarget(DamageableTarget damageableTarget)
        {
            CurrentTarget = damageableTarget;
            _hitReceiver = damageableTarget.Damageable.HitReceiver;
        }

        public void OnWeaponStateChanged(bool isActive)
        {
        }
        
        public void ShotProjectile(float initialDamage)
        {
            var projectile = Object.Instantiate(_projectilePrefab, _projectileSpawnPoint.transform.position, Quaternion.identity);
            projectile.SetupProjectile(initialDamage, CurrentTarget.Damageable, _hitReceiver).Shoot();
        }
        
        public void Dispose()
        {
        }
        
#if UNITY_EDITOR
        
        public void EDITOR_ManualValidate()
        {
            
        }
        
#endif
    }
}
