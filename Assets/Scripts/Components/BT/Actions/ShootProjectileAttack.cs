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
    public class ShootProjectileAttack : WeaponCombatAction
    {
        private ShootProjectileSetup _shootProjectileSetup;
        private List<ShotProjectileWeaponHandler> _shotProjectileWeaponHandlers;
        
        public override void Construct(IBehaviorActionContainer container)
        {
            base.Construct(container);
            _shootProjectileSetup = CommonSetup as ShootProjectileSetup;
            _shotProjectileWeaponHandlers = WeaponsSet.GetWeaponAttackHandlers<ShotProjectileWeaponHandler>();
        }

        public override void Execute()
        {
            base.Execute();
        }
        
        protected override void StartCombatAnimation()
        {
            StartReloadAnimation();
        }

        private void StartReloadAnimation()
        {
            AnimationCallbackForget = AnimationPlayer.PlayCustomAnimation(_shootProjectileSetup.ReloadAnimation, StartShootAnimation);
        }

        private void StartShootAnimation()
        {
            AnimationPlayer.PlayCustomAnimation(new AnimationClipData(targetStateName: AnimationStatesNames.Idle));
            
            AnimationPlayer.AnimationEventsListener.AnimationEventFired += OnAnimationCallback;
            AnimationCallbackForget = AnimationPlayer.PlayCustomAnimation(_shootProjectileSetup.AnimationClipData, SetCompleted);
        }
        
        private void SetCompleted()
        {
            Reset();
            CurrentStatus = TaskStatus.Success;
        }

        private void Reset()
        {
            AnimationPlayer.AnimationEventsListener.AnimationEventFired -= OnAnimationCallback;
        }

        protected override void OnAnimationCallback(AnimationEventType eventType)
        {
            if (eventType == AnimationEventType.HITSTART) 
                OnHitStart();
        }

        private void OnHitStart()
        {
            _shotProjectileWeaponHandlers.ForEach(x=>x
                .ShotProjectile(_shootProjectileSetup.AttackDamageMultiplier*WeaponStatsProvider.GetCombatStats().AttackDamage));
        }

        public override void Stop()
        {
            Reset();
            base.Stop();
        }
    }
}
