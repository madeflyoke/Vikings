using System;
using BehaviorDesigner.Runtime;
using Components;
using Components.BT;
using Components.Interfaces;
using Components.Settings;
using Interfaces;

namespace Factories.Decorators
{
    public class BehaviorTreeComponentDecorator : IEntityDecorator
    {
        private readonly BehaviorTreeComponentSettings _behaviorTreeComponentSettings;
        private readonly IReadOnlyEntity _entity;

        public BehaviorTreeComponentDecorator(BehaviorTreeComponentSettings behaviorTreeComponentSettings, IReadOnlyEntity entity)
        {
            _behaviorTreeComponentSettings = behaviorTreeComponentSettings;
            _entity = entity;
        }
        
        public IEntityComponent Decorate()
        {
            var behaviorTreeHolder = CreateBehaviorTreeComponentHolder();
            return behaviorTreeHolder;
        }

        private BehaviorTreeComponentHolder CreateBehaviorTreeComponentHolder()
        {
            var bt = _entity.GetEntityComponent<EntityHolder>().SelfTransform.gameObject.AddComponent<BehaviorTree>();
            bt.StartWhenEnabled = false;
            bt.ExternalBehavior = _behaviorTreeComponentSettings.ExternalBehaviorTree;
            
            var installer = _behaviorTreeComponentSettings.BehaviorTreeInstaller.Install(bt, 
                _behaviorTreeComponentSettings.CachedVariablesHolder, _entity, out Action onStartBehavior);
            
            bt.EnableBehavior();

            return new BehaviorTreeComponentHolder(installer, onStartBehavior);
        }
    }
}
