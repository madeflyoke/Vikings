using UnityEngine;

namespace Components.View
{
    public class HumanoidModelHolder : ModelHolder
    {
        [field: SerializeField] public Transform RightHandPoint { get; private set; }
    }
}
