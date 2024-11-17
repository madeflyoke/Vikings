using System;
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
        [field: SerializeField] public Transform TopPoint { get; private set; }

#if UNITY_EDITOR

        private void OnValidate()
        {
            OverallCollider = GetComponent<Collider>();
            TopPoint = GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "TopPoint");
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

