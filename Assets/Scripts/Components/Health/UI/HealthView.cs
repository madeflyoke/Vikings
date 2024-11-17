using System;
using UI;

namespace Components.Health.UI
{
    public class HealthView : IDisposable
    {
        private Health _health;
        private FillingBar _healthBar;

        public HealthView(Health health, FillingBar healthBar)
        {
            _health = health;
            _health.HealthEmptyEvent += OnHealthEmpty;
            _health.HealthChanged += OnHealthChanged;
            _healthBar = healthBar;
        }

        private void OnHealthChanged(float value)
        {
            _healthBar.SetFillingValue(value);
        }

        private void OnHealthEmpty()
        {
            _healthBar.SetActive(false);
        }

        public void Dispose()
        {
            _health.HealthEmptyEvent -= OnHealthEmpty;
            _health.HealthChanged -= OnHealthChanged;
        }
    }
}
