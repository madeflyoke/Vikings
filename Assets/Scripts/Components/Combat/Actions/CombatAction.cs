using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BT.Interfaces;
using BT.Shared;
using Components.Animation;
using Components.Animation.Enums;
using Components.Animation.Interfaces;
using Components.Combat.Actions.Setups;
using Components.Combat.Interfaces;
using Components.Combat.Weapons;

namespace Components.Combat.Actions
{
    public abstract class CombatAction : IBehaviorAction, IAnimationCallerHolder, IDisposable
    {
        private CommonCombatActionSetup CommonSetup { get; set; }
        protected ICombatStatsProvider CombatStatsProvider;
        
        public AnimationCaller AnimationCaller { get; private set; }
        protected WeaponSet WeaponsSet;

        public virtual void Initialize(CommonCombatActionSetup commonSetup, WeaponSet weaponsSet)
        {
            CommonSetup = commonSetup;
            AnimationCaller = new AnimationCaller();
            WeaponsSet = weaponsSet;
        }

        public void SetCombatStatsProvider(ICombatStatsProvider combatStatsProvider)
        {
            CombatStatsProvider = combatStatsProvider;
        }
        
        
        protected virtual void OnAnimationCallback(AnimationEventType eventType) { }
       
        public abstract TaskStatus GetCurrentStatus();
        public abstract void Execute();

        public virtual void Dispose()
        {
        }
    }
}