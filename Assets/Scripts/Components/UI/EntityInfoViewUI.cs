using System;
using Components.Combat.Interfaces;
using Components.Interfaces;
using UniRx;
using UnityEngine;

namespace Components.UI
{
    public class EntityInfoViewUI : MonoBehaviour, IEntityComponent
    {
        private Camera _camera;
        private bool _initialized;
        
        public void InitializeComponent()
        {
            _camera = Camera.main;
            _initialized = true;
        }
        
        private void Update()
        {
            if (_initialized==false)
            {
                return;
            }
            
            transform.LookAt(new Vector3(transform.position.x, _camera.transform.position.y, _camera.transform.position.z));
        }

        public void Dispose()
        {
        }
    }
}
