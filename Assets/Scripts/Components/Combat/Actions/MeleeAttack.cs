using BehaviorDesigner.Runtime.Tasks;
using Components.Animation.Enums;
using Components.Combat.Actions.Setups;
using Components.Combat.Interfaces;

namespace Components.Combat.Actions
{
    public class MeleeAttack : CombatAction
    {
        private bool _finished;
        private MeleeAttackSetup _meleeSetup;
        
        public override void Initialize(CommonCombatActionSetup commonSetup) 
        {
            base.Initialize(commonSetup);
            _meleeSetup = commonSetup as MeleeAttackSetup;
        }

        public override TaskStatus GetCurrentStatus()
        {
            if (_finished)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }

        public override void Execute()
        {
            _finished = false;
            AnimationCaller.CallOnAnimation?.Invoke(AnimationCaller, _meleeSetup.AnimationClipData);
        }

        private void SetFinished()
        {
            _finished = true;
        }

        protected override void OnAnimationCallback(AnimationEventType eventType)
        {
            switch (eventType)
            {
                case AnimationEventType.END:
                    SetFinished();
                    break;
            }
        }
    }
}
