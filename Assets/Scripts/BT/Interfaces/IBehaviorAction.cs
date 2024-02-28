using BehaviorDesigner.Runtime.Tasks;

namespace BT.Interfaces
{
    public interface IBehaviorAction
    {
        public abstract TaskStatus Execute();
    }
}
