using Components;
using Components.Interfaces;
using Components.Movement;
using Components.Settings;
using Interfaces;
using UnityEngine.AI;

namespace Factories.Decorators
{
    public class NavMeshMovementComponentDecorator : IEntityDecorator
    {
        private readonly MovementComponentSettings _movementComponentSettings;
        private readonly EntityHolder _entityHolder;
        
        public NavMeshMovementComponentDecorator(EntityHolder entityHolder, MovementComponentSettings movementComponentSettings)
        {
            _entityHolder = entityHolder;
            _movementComponentSettings = movementComponentSettings;
        }
        
        public IEntityComponent Decorate()
        {
            var movementComponent = CreateMovementComponent();
            return movementComponent;
        }

        private NavMeshMovementComponent CreateMovementComponent()
        {
            var navMeshAgent = _entityHolder.SelfTransform.gameObject.AddComponent<NavMeshAgent>();

            if (_movementComponentSettings.NavMeshAgentTemplate!=null)
            {
                _movementComponentSettings.NavMeshAgentTemplate.ApplyTo(navMeshAgent);
            }
            
            return new NavMeshMovementComponent(navMeshAgent);
        }
    }
}
