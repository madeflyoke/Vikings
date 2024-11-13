using UnityEngine;

namespace Components.Combat.Interfaces
{
    public interface IHitReceiver
    {
        public Collider OverallCollider { get; }
    }
}
