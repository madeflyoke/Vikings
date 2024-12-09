using System;
using Components.Combat.Interfaces;
using Components.Combat.Weapons;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Utility;

namespace Components.Combat.Projectiles
{
    [RequireComponent(typeof(Collider))]
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] private ProjectileStats _baseProjectileStats;
        private float _damage;
        private IDamageable _targetDamageable;
        private IHitReceiver _hitReceiver;
        private Vector3 Destination => _hitReceiver.OverallCollider.bounds.center;

        private bool _canMove;

        public Projectile SetupProjectile(float damage, IDamageable damageable, IHitReceiver hitReceiver)
        {
            _damage = damage * _baseProjectileStats.DamageMultiplier;
            _targetDamageable = damageable;
            _hitReceiver = hitReceiver;
            transform.forward = (Destination - transform.position).normalized;
            return this;
        }

        [Button]
        public void Shoot()
        {
            _canMove = true;
        }

        private void FixedUpdate()
        {
            if (_canMove)
            {
                var dir = (Destination - transform.position).normalized;
                transform.forward = dir;
                transform.Translate(dir* Time.fixedDeltaTime * _baseProjectileStats.Speed,Space.World);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other==_hitReceiver.OverallCollider)
            {
                _canMove = false;
                _targetDamageable.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            _canMove = false;
        }
    }
}