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
        [field: SerializeField] public Collider HitCollider { get; private set; }
        [SerializeField] private Renderer _renderer;
        [SerializeField] private List<Material> _relatedMaterials;

#if UNITY_EDITOR

        private void OnValidate()
        {
            _renderer = GetComponentInChildren<Renderer>();
            _relatedMaterials = _renderer.sharedMaterials.ToList();
            HitCollider = GetComponent<Collider>();
        }
#endif
        public void InitializeComponent()
        {
            
        }
    }
}

