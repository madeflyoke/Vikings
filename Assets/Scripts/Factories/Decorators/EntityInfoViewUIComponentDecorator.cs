using Builders.Utility;
using Components.Interfaces;
using Components.UI;
using Interfaces;
using UnityEngine;
using Utility;

namespace Factories.Decorators
{
    public class EntityInfoViewUIComponentDecorator : IEntityDecorator
    {
        private readonly Transform _spawnPoint;

        public EntityInfoViewUIComponentDecorator(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
        }
        
        private EntityInfoViewUI CreateInfoView()
        {
            GameObjectComponentBuilder<EntityInfoViewUI> goBuilder = new ();
            
            var entityInfoView = goBuilder
                .SetPrefab(Resources.Load<EntityInfoViewUI>(ResourcesPaths.Components.EntityInfoViewUIPath))
                .SetParent(_spawnPoint)
                .WithOriginalPositionAndRotation()
                .Build();
            
            return entityInfoView;
        }
        
        public IEntityComponent Decorate()
        {
           var infoView = CreateInfoView();
           return infoView;
        }
    }
}
