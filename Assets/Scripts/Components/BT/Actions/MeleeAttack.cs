using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Components.Animation;
using Components.Animation.Enums;
using Components.BT.Actions.Containers.Interfaces;
using Components.BT.Actions.Setups;
using Components.Combat.Weapons.Handlers;
using Utility;

namespace Components.BT.Actions
{
    public class MeleeAttack : WeaponCombatAction
    {
        private bool _wasHit;
        private MeleeAttackSetup _meleeSetup;
        private List<MeleeAttackWeaponHandler> _meleeAttackHandlers;
        
        public override void Construct(IBehaviorActionContainer container)
        {
            base.Construct(container);
            _meleeSetup = CommonSetup as MeleeAttackSetup;
            _meleeAttackHandlers = WeaponsSet.GetWeaponAttackHandlers<MeleeAttackWeaponHandler>();
        }

        public override void Execute()
        {
            base.Execute();
        }

        protected override void StartCombatAnimation()
        {
            AnimationPlayer.AnimationEventsListener.AnimationEventFired += OnAnimationCallback;
            _meleeAttackHandlers.ForEach(x => x.HitEvent += OnWeaponHit);
            
            AnimationCallbackForget = AnimationPlayer.PlayCustomAnimation(_meleeSetup.AnimationClipData, SetCompleted);
        }

        private void SetCompleted()
        {
            Reset();
            CurrentStatus = TaskStatus.Success;
        }

        private void Reset()
        {
            _wasHit = false;
            AnimationPlayer.AnimationEventsListener.AnimationEventFired -= OnAnimationCallback;
            _meleeAttackHandlers.ForEach(x => x.HitEvent -= OnWeaponHit);
        }

        protected override void OnAnimationCallback(AnimationEventType eventType)
        {
            switch (eventType)
            {
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
            _meleeAttackHandlers.ForEach(x => x.SetColliderActive(true));
        }

        private void OnHitEnd()
        {
            _meleeAttackHandlers.ForEach(x => x.SetColliderActive(false));
        }

        private void OnWeaponHit(DamageableTarget damageableTarget)
        {
            if (_wasHit == false)
            {
                _wasHit = true;
                var damage = WeaponStatsProvider.GetCombatStats().AttackDamage * _meleeSetup.AttackDamageMultiplier;
                damageableTarget.Damageable.TakeDamage(damage);
            }
        }

        public override void Stop()
        {
            Reset();
            base.Stop();
        }
    }
}