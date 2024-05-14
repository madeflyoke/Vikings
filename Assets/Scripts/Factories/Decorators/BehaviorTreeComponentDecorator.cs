using System;
using BehaviorDesigner.Runtime;
using Components.BT;
using Components.BT.Interfaces;
using Components.Interfaces;
using Components.Settings;
using Interfaces;

namespace Factories.Decorators
{
    public class BehaviorTreeComponentDecorator<TInstaller> : IEntityDecorator where TInstaller : IBehaviorTreeInstaller
    {
        private readonly BehaviorTreeComponentSettings _behaviorTreeComponentSettings;
        private readonly IBehaviorTreeInstallerData _installerData;

        public BehaviorTreeComponentDecorator(BehaviorTreeComponentSettings behaviorTreeComponentSettings, 
            IBehaviorTreeInstallerData installerData)
        {
            _behaviorTreeComponentSettings = behaviorTreeComponentSettings;
            _installerData = installerData;
        }
        
        public IEntityComponent Decorate()
        {
            var behaviorTreeHolder = CreateBehaviorTreeComponentHolder();
            return behaviorTreeHolder;
        }

        private BehaviorTreeComponentHolder CreateBehaviorTreeComponentHolder()
        {
            var bt = _installerData.EntityHolder.SelfTransform.gameObject.AddComponent<BehaviorTree>();
            bt.StartWhenEnabled = false;
            bt.ExternalBehavior = _behaviorTreeComponentSettings.ExternalBehaviorTree;
            
            var installer = Activator.CreateInstance<TInstaller>(); //individual installers from settings for each unit

            if (installer==null)
            {
                throw new Exception("Invalid behavior tree installer, look up in config!");
            }
            
            installer.Install(bt, _installerData);
            
            return new BehaviorTreeComponentHolder(bt);
        }
    }
}
