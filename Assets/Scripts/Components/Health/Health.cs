using System;
using UnityEngine;

namespace Components.Health
{
    public class Health
    {
        public int CurrentHealth { get; private set; }
        private readonly int _maxHealth;

        public Health(int maxHealth)
        {
            _maxHealth = maxHealth;
            RestoreHealth();
        }

        public void AddHealth(int value)
        {
            CurrentHealth += value;
        }

        public void SubtractHealth(int value)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - value, 0, _maxHealth);
            Debug.LogWarning($"Damage taken: {value}, health: {CurrentHealth}/{_maxHealth}");
        }

        public void RestoreHealth()
        {
            CurrentHealth = _maxHealth;
        }
    }
}
