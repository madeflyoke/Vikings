using System;
using Components.Combat.Interfaces;
using Components.Health.UI;
using Components.Interfaces;

namespace Components.Health
{
    public class HealthComponent: IDamageable, IEntityComponent
    {
        public event Action<IDamageable> DeadEvent;
        
        public IHitReceiver HitReceiver { get; }
        public bool IsAlive { get; private set; }

        private readonly Health _health;
        private readonly HealthView _healthView;
        
        public HealthComponent( Health health, HealthView healthView, IHitReceiver hitReceiver)
        {
            _health = health;
            _healthView = healthView;
            
            _health.HealthEmptyEvent += OnHealthEmpty;
            HitReceiver = hitReceiver;
            IsAlive = true;
        }
        
        public void TakeDamage(float value)
        {
            _health.SubtractHealth(value);
            if (_health.CurrentHealth == 0)
            {
                IsAlive = false;
                DeadEvent?.Invoke(this);
            }
        }

        private void OnHealthEmpty()
        {
            IsAlive = false;
            DeadEvent?.Invoke(this);
        }

        public void InitializeComponent()
        {
            
        }

        public void Dispose()
        {
            _health.HealthEmptyEvent -= OnHealthEmpty;
        }
    }
}
