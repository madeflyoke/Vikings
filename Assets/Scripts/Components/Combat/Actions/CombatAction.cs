using BehaviorDesigner.Runtime.Tasks;
using BT.Interfaces;
using Components.Animation;
using Components.Animation.Enums;
using Components.Combat.Actions.Setups;
using Components.Combat.Interfaces;

namespace Components.Combat.Actions
{
    public abstract class CombatAction : IBehaviorAction
    {
        protected ICombatStatsCopyProvider CombatStatsCopyProvider;
        
        protected AnimationCaller AnimationCaller;
        
        public virtual void Initialize(CommonCombatActionSetup commonSetup)
        {
            AnimationCaller = new AnimationCaller();
            AnimationCaller.Callback += OnAnimationCallback;
        }

        public AnimationCaller GetAnimationCaller()
        {
            return AnimationCaller;
        }

        public void SetCombatStatsProvider(ICombatStatsCopyProvider combatStatsCopyProvider)
        {
            CombatStatsCopyProvider = combatStatsCopyProvider;
        }
        
        protected virtual void OnAnimationCallback(AnimationEventType eventType) { }
       
        public abstract TaskStatus GetCurrentStatus();
        public abstract void Execute();
        
    }
}