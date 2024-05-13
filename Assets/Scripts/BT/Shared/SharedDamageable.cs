using BehaviorDesigner.Runtime;
using Components.Health;
using Interfaces;

namespace BT.Shared
{
    public class SharedDamageable : SharedVariable<IDamageable>
    {
        public static implicit operator SharedDamageable(HealthComponent value) { return new SharedDamageable{ Value = value }; } 
        //all IDamageable will be declared here
    }
}
