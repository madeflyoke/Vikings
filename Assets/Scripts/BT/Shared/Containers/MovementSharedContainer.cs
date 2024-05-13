using System;
using BehaviorDesigner.Runtime;

namespace BT.Shared.Containers
{
    [Serializable]
    public class MovementSharedContainer
    {
        public SharedVector3 CurrentDestinationPoint;
    }
    
    public class MovementSharedContainerVariable : SharedVariable<MovementSharedContainer>
    {
        public static implicit operator MovementSharedContainerVariable(MovementSharedContainer value) 
        { return new MovementSharedContainerVariable{ Value = value }; }
    }
}
