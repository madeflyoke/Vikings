using Components.Interfaces;
using UnityEngine;

namespace Components
{
    public class EntityHolder : MonoBehaviour, IEntityComponent
    {
        [field: SerializeField] public Transform ViewTransform { get; private set; }
        [field:SerializeField, HideInInspector] public Transform SelfTransform { get; private set; }

        
#if UNITY_EDITOR

        private void OnValidate()
        {
            SelfTransform = transform;
        }

#endif
        public void InitializeComponent()
        {
            
        }
    }
}