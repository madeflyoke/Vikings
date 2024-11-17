using Builders.Utility;
using Components.Combat.Interfaces;
using Components.Health;
using Components.Health.UI;
using Components.Interfaces;
using Components.Settings;
using Interfaces;
using UI;
using UnityEngine;
using Utility;

namespace Factories.Decorators
{
    public class HealthComponentDecorator : IEntityDecorator
    {
        private readonly HealthComponentSettings _healthComponentSettings;
        private readonly IHitReceiver _hitReceiver;
        private readonly Color _teamColor;
        private readonly Transform _viewParent;
        
        public HealthComponentDecorator(HealthComponentSettings healthComponentSettings, IHitReceiver hitReceiver, 
            Transform viewParent,
            Color teamColor)
        {
            _healthComponentSettings = healthComponentSettings;
            _hitReceiver = hitReceiver;
            _teamColor = teamColor;
            _viewParent = viewParent;
        }
        
        private void CreateHealthParts(out Health health, out HealthView healthView)
        {
            GameObjectComponentBuilder<FillingBar> goBuilder = new ();
            
            var fillingBar = goBuilder
                .SetPrefab(Resources.Load<FillingBar>(ResourcesPaths.Components.HealthViewPath))
                .SetParent(_viewParent)
                .WithOriginalPositionAndRotation()
                .Build();
            
            fillingBar.SetColor(_teamColor);
            fillingBar.Initialize(_healthComponentSettings.BaseHealth);
            
            health = new Health(_healthComponentSettings.BaseHealth);
            healthView = new HealthView(health, fillingBar);
        }
        
        public IEntityComponent Decorate()
        {
            var healthController = CreateHealthController();
            return healthController;
        }

        private HealthComponent CreateHealthController()
        {
            CreateHealthParts(out Health health, out HealthView healthView);
            return new HealthComponent(health, healthView, _hitReceiver);
        }
    }
}
