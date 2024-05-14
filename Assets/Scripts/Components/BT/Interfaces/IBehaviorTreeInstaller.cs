using BehaviorDesigner.Runtime;

namespace Components.BT.Interfaces
{
    public interface IBehaviorTreeInstaller
    {
        public void Install(BehaviorTree behaviorTree, IBehaviorTreeInstallerData behaviorTreeInstallerData);
    }
}
