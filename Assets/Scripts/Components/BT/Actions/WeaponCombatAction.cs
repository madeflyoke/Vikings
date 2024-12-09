using System;
using BehaviorDesigner.Runtime.Tasks;
using Components.Animation.Enums;
using Components.Animation.Interfaces;
using Components.BT.Actions.Containers;
using Components.BT.Actions.Containers.Interfaces;
using Components.BT.Actions.Interfaces;
using Components.BT.Actions.Setups;
using Components.BT.Interfaces;
using Components.Combat.Interfaces;
using Components.Combat.Weapons;

namespace Components.BT.Actions
{
    public abstract class WeaponCombatAction : IBehaviorAction, IBehaviorActionInjectedByContainer, IDisposable
    {
        public TaskStatus CurrentStatus { get; protected set; }

        protected IWeaponStatsProvider WeaponStatsProvider;
        protected  IAnimationPlayer AnimationPlayer;
        protected IDisposable AnimationCallbackForget;
        protected CommonWeaponActionSetup CommonSetup;
        protected WeaponSet WeaponsSet;
        
        public virtual void Construct(IBehaviorActionContainer container)
        {
            var specifiedContainer = container as CombatBehaviorActionContainer;
            WeaponStatsProvider = specifiedContainer.WeaponsHolder;
            AnimationPlayer = specifiedContainer.AnimationPlayer;
            CommonSetup = specifiedContainer.TargetActionSetup as CommonWeaponActionSetup;
            WeaponsSet = specifiedContainer.WeaponsHolder.GetWeaponSetByConditions(CommonSetup.Conditions);
        }
        
        public virtual void Execute()
        {
            CurrentStatus = TaskStatus.Running;
            AnimationCallbackForget = null;

            WeaponsSet.CallOnActivate();
            StartCombatAnimation();
        }

        public virtual void Stop()
        {
            CurrentStatus = TaskStatus.Inactive;
            AnimationCallbackForget?.Dispose();
        }

        public void NotifyInterrupt()
        {
            //intentionally blank
        }

        public virtual void Dispose()
        {
            AnimationCallbackForget?.Dispose();
        }
        
        protected abstract void StartCombatAnimation();
        protected abstract void OnAnimationCallback(AnimationEventType eventType);
    }
}