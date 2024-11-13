using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
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
        public CommonCombatActionSetup CommonSetup { get; private set; }
        protected ICombatStatsProvider CombatStatsProvider;
        
        protected AnimationCaller AnimationCaller;
        protected WeaponSet WeaponsSet;

        public virtual void Initialize(CommonCombatActionSetup commonSetup, WeaponSet weaponsSet)
        {
            CommonSetup = commonSetup;
            AnimationCaller = new AnimationCaller();
            WeaponsSet = weaponsSet;
        }

        public AnimationCaller GetAnimationCaller()
        {
            return AnimationCaller;
        }

        public void SetCombatStatsProvider(ICombatStatsProvider combatStatsProvider)
        {
            CombatStatsProvider = combatStatsProvider;
        }
        
        
        protected virtual void OnAnimationCallback(AnimationEventType eventType) { }
       
        public abstract TaskStatus GetCurrentStatus();
        public abstract void Execute();
        
    }
}