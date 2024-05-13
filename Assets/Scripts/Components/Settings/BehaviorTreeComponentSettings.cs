using BehaviorDesigner.Runtime;
using BT.Tools;
using Components.BT.Interfaces;
using Components.Settings.Interfaces;

namespace Components.Settings
{
    public class BehaviorTreeComponentSettings : IComponentSettings
    {
        public ExternalBehaviorTree ExternalBehaviorTree;
        public BehaviorTreeCachedVariablesHolder CachedVariablesHolder;
        public IBehaviorTreeInstaller BehaviorTreeInstaller;
    }
}
