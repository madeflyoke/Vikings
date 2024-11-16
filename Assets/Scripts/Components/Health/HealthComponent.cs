using System;
using Components.Combat.Interfaces;
using Components.Interfaces;
using Interfaces;
using UnityEngine;

namespace Components.Health
{
    public class HealthComponent: IDamageable, IEntityComponent
    {
        public event Action<IDamageable> DeadEvent;
        
        public IHitReceiver HitReceiver { get; }
        public bool IsAlive { get; private set; }

        private readonly Health _health;
        
        //health view

        public HealthComponent(int maxHealth, IHitReceiver hitReceiver)
        {
            _health = new Health(maxHealth);
            HitReceiver = hitReceiver;
            IsAlive = true;
        }
        
        public void TakeDamage(int value)
        {
            _health.SubtractHealth(value);
            if (_health.CurrentHealth == 0)
            {
                IsAlive = false;
                DeadEvent?.Invoke(this);
            }
        }

        public void InitializeComponent()
        {
            
        }

        public void Dispose()
        {
        }
    }
}
