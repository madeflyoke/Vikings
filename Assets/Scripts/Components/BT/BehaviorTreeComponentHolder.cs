using BehaviorDesigner.Runtime;
using Components.Interfaces;

namespace Components.BT
{
    public class BehaviorTreeComponentHolder : IEntityComponent
    {
        private readonly BehaviorTree _behaviorTree;

        public BehaviorTreeComponentHolder(BehaviorTree behaviorTree)
        {
            _behaviorTree = behaviorTree;
        }

        public void InitializeComponent()
        {
            _behaviorTree.EnableBehavior();
        }

        public void Dispose()
        {
        }
    }
}
