using System;

namespace Interfaces
{
    public interface IDamageable
    {
        public event Action<IDamageable> DeadEvent;
        public abstract bool IsAlive { get; }
        public abstract void TakeDamage(int value);
    }
}
