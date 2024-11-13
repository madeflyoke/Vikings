using System.Collections.Generic;
using System.Linq;
using Components.Combat.Interfaces;
using Components.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.View
{
    public class ModelHolder : MonoBehaviour, IEntityComponent, IHitReceiver
    {
        [field: SerializeField] public Collider OverallCollider { get; private set; }

#if UNITY_EDITOR

        private void OnValidate()
        {
            OverallCollider = GetComponent<Collider>();
        }
#endif
        public void InitializeComponent()
        {
            
        }

        public void Dispose()
        {
        }
    }
}

