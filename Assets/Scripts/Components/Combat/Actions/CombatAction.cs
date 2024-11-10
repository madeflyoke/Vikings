﻿using BehaviorDesigner.Runtime.Tasks;
using BT.Interfaces;
using BT.Shared;
using Components.Animation;
using Components.Animation.Enums;
using Components.Combat.Actions.Setups;
using Components.Combat.Interfaces;
using Components.Combat.Weapons;

namespace Components.Combat.Actions
{
    public abstract class CombatAction : IBehaviorAction
    {
        protected ICombatStatsCopyProvider CombatStatsCopyProvider;
        
        protected AnimationCaller AnimationCaller;
        protected Weapon CurrentWeapon;

        public virtual void Initialize(CommonCombatActionSetup commonSetup, Weapon weapon)
        {
            AnimationCaller = new AnimationCaller();
            CurrentWeapon = weapon;
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