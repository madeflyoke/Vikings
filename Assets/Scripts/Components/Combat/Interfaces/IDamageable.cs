using System;

namespace Components.Combat.Interfaces
{
    public interface IDamageable
    {
        public event Action<IDamageable> DeadEvent;
        public IHitReceiver HitReceiver { get; }
        public abstract bool IsAlive { get; }
        public abstract void TakeDamage(int value);
    }
}
