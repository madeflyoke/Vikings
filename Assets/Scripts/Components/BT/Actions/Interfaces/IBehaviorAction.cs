using BehaviorDesigner.Runtime.Tasks;
using Factories.Interfaces;

namespace Components.BT.Actions.Interfaces
{
    public interface IBehaviorAction : IFactoryProduct
    {
        public TaskStatus CurrentStatus { get; }
        public void Execute();
        public void Stop();
        public void NotifyInterrupt();
    }
}
