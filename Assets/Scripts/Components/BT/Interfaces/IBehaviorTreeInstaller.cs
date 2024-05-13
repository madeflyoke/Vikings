using System;
using BehaviorDesigner.Runtime;
using BT.Tools;
using Interfaces;

namespace Components.BT.Interfaces
{
    public interface IBehaviorTreeInstaller
    {
        public IBehaviorTreeInstaller Install(BehaviorTree behaviorTree, BehaviorTreeCachedVariablesHolder cachedVariablesHolder, IReadOnlyEntity entity
        ,out Action startTrigger);
    }
}
