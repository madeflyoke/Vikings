using System;
using Components.BT.Interfaces;
using Components.Interfaces;

namespace Components.BT
{
    public class BehaviorTreeComponentHolder : IEntityComponent
    {
        private IBehaviorTreeInstaller _installer;
        private readonly Action _onStartBehavior;

        public BehaviorTreeComponentHolder(IBehaviorTreeInstaller installer, Action onStartBehavior)
        {
            _installer = installer;
            _onStartBehavior = onStartBehavior;
        }

        public void StartBehavior()
        {
            _onStartBehavior?.Invoke();
        }
    }
}
