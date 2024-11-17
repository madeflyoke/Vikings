using System;
using UnityEngine;

namespace Components.Health
{
    public class Health
    {
        public event Action HealthEmptyEvent;
        public event Action<float> HealthChanged;
        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }

        public Health(float maxHealth)
        {
            MaxHealth = maxHealth;
            RestoreHealth();
        }

        public void AddHealth(float value)
        {
            CurrentHealth += value;
            HealthChanged?.Invoke(CurrentHealth);
        }

        public void SubtractHealth(float value)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - value, 0, MaxHealth);
            HealthChanged?.Invoke(CurrentHealth);
            if (CurrentHealth == 0)
            {
                HealthEmptyEvent?.Invoke();
            }
            Debug.LogWarning($"Damage taken: {value}, health: {CurrentHealth}/{MaxHealth}");
        }

        public void RestoreHealth()
        {
            CurrentHealth = MaxHealth;
            HealthChanged?.Invoke(CurrentHealth);
        }
    }
}
