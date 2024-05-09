using Components.Health;
using Components.Interfaces;
using Components.Settings;
using Interfaces;

namespace Factories.Decorators
{
    public class HealthComponentDecorator : IEntityDecorator
    {
        private readonly HealthComponentSettings _healthComponentSettings;
        
        public HealthComponentDecorator(HealthComponentSettings healthComponentSettings)
        {
            _healthComponentSettings = healthComponentSettings;
        }
        
        public IEntityComponent Decorate()
        {
            var healthController = CreateHealthController();
            return healthController;
        }

        private HealthComponent CreateHealthController()
        {
            return new HealthComponent (_healthComponentSettings.BaseHealth);
        }
    }
}
