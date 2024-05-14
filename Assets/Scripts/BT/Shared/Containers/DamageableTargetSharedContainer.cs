using System;
using BehaviorDesigner.Runtime;

namespace BT.Shared.Containers
{
    [Serializable]
    public class DamageableTargetSharedContainer
    {
        public SharedDamageable TargetDamageable;
        public SharedTransform TargetTr;
    }
    
    public class DamageableTargetSharedContainerVariable : SharedVariable<DamageableTargetSharedContainer>
    {
        public static implicit operator DamageableTargetSharedContainerVariable(DamageableTargetSharedContainer value) 
        { return new DamageableTargetSharedContainerVariable{ Value = value }; }
    }
}
