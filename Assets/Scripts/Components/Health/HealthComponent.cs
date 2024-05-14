using System;
using Components.Interfaces;
using Interfaces;

namespace Components.Health
{
    public class HealthComponent: IDamageable, IEntityComponent
    {
        public bool IsAlive { get; private set; }

        public event Action HealthEmptyEvent; 
        private readonly Health _health;
        
        //health view

        public HealthComponent(int maxHealth)
        {
            _health = new Health(maxHealth);
            IsAlive = true;
        }
        
        public void TakeDamage(int value)
        {
            _health.SubtractHealth(value);
            if (_health.CurrentHealth == 0)
            {
                IsAlive = false;
                HealthEmptyEvent?.Invoke();
            }
        }

        public void InitializeComponent()
        {
            
        }
    }
}
