using UnityEngine;
using UnityEngine.AI;

namespace Components.Movement.Interfaces
{
    public interface IMovementProvider
    {
        public void SetMovementPoint(Vector3 destinationPoint);
        public void StartMovement();
        public void StopMovement();

        public NavMeshPath CalculatePath(Vector3 destinationPos);
        public void SetRelatedComponentActive(bool value);
    }
}
