using BehaviorDesigner.Runtime.Tasks;
using Components.Animation.Enums;
using Components.Combat.Actions.Setups;
using Components.Combat.Weapons;
using Components.Combat.Weapons.Handlers;
using Utility;

namespace Components.Combat.Actions
{
    public class MeleeAttack : CombatAction
    {
        private bool _completed;
        private bool _wasHit;
        private MeleeAttackSetup _meleeSetup;
        private MeleeAttackWeaponHandler _attackHandler;

        public override void Initialize(CommonCombatActionSetup commonSetup, Weapon weapon) 
        {
            base.Initialize(commonSetup, weapon);
            _meleeSetup = commonSetup as MeleeAttackSetup;
            _attackHandler = weapon.GetWeaponActionHandler<MeleeAttackWeaponHandler>();
        }

        public override TaskStatus GetCurrentStatus()
        {
            if (_completed)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }

        public override void Execute()
        {
            _completed = false;
            
            StartAttackAnimation();
        }

        private void StartAttackAnimation()
        {
            AnimationCaller.AnimationsEventsListener.AnimationEventFired += OnAnimationCallback;
            _attackHandler.HitEvent += OnWeaponHit;
            
            AnimationCaller.CallOnAnimation?.Invoke(AnimationCaller, _meleeSetup.AnimationClipData);
        }
        
        private void SetCompleted()
        {
            _completed = true;
            
            _wasHit = false;
            AnimationCaller.AnimationsEventsListener.AnimationEventFired -= OnAnimationCallback;
            _attackHandler.HitEvent -= OnWeaponHit;
        }

        protected override void OnAnimationCallback(AnimationEventType eventType)
        {
            switch (eventType)
            {
                case AnimationEventType.END:
                    SetCompleted();
                    break;
                case AnimationEventType.HITSTART:
                    OnHitStart();
                    break;
                case AnimationEventType.HITEND:
                    OnHitEnd();
                    break;
            }
        }

        private void OnHitStart()
        {
            _attackHandler.SetColliderActive(true);
        }

        private void OnHitEnd()
        {
            _attackHandler.SetColliderActive(false);
        }
        
        private void OnWeaponHit(DamageableTarget damageableTarget)
        {
            if (_wasHit==false)
            {
                _wasHit = true;
                var damage = CombatStatsCopyProvider.GetCombatStatsCopy().AttackDamage * _meleeSetup.AttackDamageMultiplier;
                damageableTarget.Damageable.TakeDamage(damage);
            }
        }
    }
}
