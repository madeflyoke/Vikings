using BehaviorDesigner.Runtime.Tasks;
using BT.Shared;

namespace BT.Nodes.Actions
{
    public class SelfValidate : Conditional
    {
        private SharedDamageable _selfDamageable;

        public void SetSharedVariables(SharedDamageable targetDamageable)
        {
            _selfDamageable = targetDamageable;
        }
        
        public override TaskStatus OnUpdate()
        {
            return Validate() ? TaskStatus.Success : TaskStatus.Failure;
        }

        private bool Validate()
        {
            return _selfDamageable.Value.IsAlive;
        }
    }
}
