using System.Collections.Generic;
using System.Linq;
using Components.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.View
{
    public class ModelHolder : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private List<Material> _relatedMaterials;

#if UNITY_EDITOR

        private void OnValidate()
        {
            _renderer = GetComponentInChildren<Renderer>();
            _relatedMaterials = _renderer.sharedMaterials.ToList();
        }
#endif
        public void InitializeComponent()
        {
            
        }
    }
}

