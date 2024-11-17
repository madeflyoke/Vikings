using BT.Interfaces;

namespace Components.BT.Interfaces
{
    public interface IBehaviorTreeInstallerData
    {
        public IBehaviorTreeStarter BehaviorTreeStarter { get; set; }
        public EntityHolder EntityHolder { get; set; }
    }
}