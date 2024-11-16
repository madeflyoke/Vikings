using Components.Combat.Interfaces;
using Components.Health;
using Components.Interfaces;
using Components.Settings;
using Interfaces;

namespace Factories.Decorators
{
    public class HealthComponentDecorator : IEntityDecorator
    {
        private readonly HealthComponentSettings _healthComponentSettings;
        private readonly IHitReceiver _hitReceiver;
        
        public HealthComponentDecorator(HealthComponentSettings healthComponentSettings, IHitReceiver hitReceiver)
        {
            _healthComponentSettings = healthComponentSettings;
            _hitReceiver = hitReceiver;
        }
        
        public IEntityComponent Decorate()
        {
            var healthController = CreateHealthController();
            return healthController;
        }

        private HealthComponent CreateHealthController()
        {
            return new HealthComponent(_healthComponentSettings.BaseHealth, _hitReceiver);
        }
    }
}
