using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace BT.Shared.Containers
{
    [Serializable]
    public class SelfGeneralDataSharedContainer
    {
        public SharedDamageable SelfDamageable;
        public SharedTransform SelfTransform;
    }
    
    public class SelfGeneralDataSharedContainerVariable : SharedVariable<SelfGeneralDataSharedContainer>
    {
        public static implicit operator SelfGeneralDataSharedContainerVariable(SelfGeneralDataSharedContainer value) { return new SelfGeneralDataSharedContainerVariable{ Value = value }; }
    }
}
