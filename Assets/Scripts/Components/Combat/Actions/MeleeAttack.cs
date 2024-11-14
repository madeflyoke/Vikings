using System.Collections.Generic;
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
        private List<MeleeAttackWeaponHandler> _meleeAttackHandlers;

        public override void Initialize(CommonCombatActionSetup commonSetup, WeaponSet weaponsSet) 
        {
            base.Initialize(commonSetup, weaponsSet);
            _meleeSetup = commonSetup as MeleeAttackSetup;
            
            _meleeAttackHandlers = weaponsSet.GetWeaponAttackHandlers<MeleeAttackWeaponHandler>();
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
            WeaponsSet.CallOnActivate();
            StartAttackAnimation();
        }

        private void StartAttackAnimation()
        {
            AnimationCaller.AnimationsEventsListener.AnimationEventFired += OnAnimationCallback;
            _meleeAttackHandlers.ForEach(x=>x.HitEvent += OnWeaponHit);
            
            AnimationCaller.CallOnAnimation?.Invoke(AnimationCaller, _meleeSetup.AnimationClipData);
        }
        
        private void SetCompleted()
        {
            _wasHit = false;
            AnimationCaller.AnimationsEventsListener.AnimationEventFired -= OnAnimationCallback;
            _meleeAttackHandlers.ForEach(x=>x.HitEvent -= OnWeaponHit);
            _completed = true;
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
            _meleeAttackHandlers.ForEach(x=>x.SetColliderActive(true));
        }

        private void OnHitEnd()
        {
            _meleeAttackHandlers.ForEach(x=>x.SetColliderActive(false));
        }
        
        private void OnWeaponHit(DamageableTarget damageableTarget)
        {
            if (_wasHit==false)
            {
                _wasHit = true;
                var damage = CombatStatsProvider.GetCurrentCombatStats().AttackDamage * _meleeSetup.AttackDamageMultiplier;
                damageableTarget.Damageable.TakeDamage(damage);
            }
        }
    }
}
