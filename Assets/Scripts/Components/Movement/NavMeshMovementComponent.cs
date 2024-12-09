using Components.Interfaces;
using Components.Movement.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Components.Movement
{
    public class NavMeshMovementComponent : IEntityComponent, IMovementProvider
    {
        private readonly NavMeshAgent _agent;

        public NavMeshMovementComponent(NavMeshAgent agent)
        {
            _agent = agent;
        }
        
        public void InitializeComponent()
        {
            StopMovement();
        }
        
        public void SetMovementPoint(Vector3 destinationPoint)
        {
            _agent.destination = destinationPoint;
        }

        public void StartMovement()
        {
            if (_agent.isStopped)
            {
                _agent.isStopped = false;
            }
        }
        
        public void StopMovement()
        {
            if (_agent.isStopped==false)
            {
                _agent.isStopped = true;
            }
        }

        public NavMeshPath CalculatePath(Vector3 destinationPos)
        {
            var path = new NavMeshPath();
            _agent.CalculatePath(destinationPos, path);
            return path;
        }

        public void SetRelatedComponentActive(bool value)
        {
            _agent.enabled = value;
        }
        
        public void Dispose()
        {
        }
    }
}
