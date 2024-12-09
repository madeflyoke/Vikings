using System;
using Components.Animation;
using Components.BT.Actions.Conditions;
using Components.BT.Actions.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.BT.Actions.Setups
{
    [Serializable]
    public abstract class CommonWeaponActionSetup : IBehaviorActionSetup
    {
        public CombatActionConditions Conditions;
        
        public AnimationClipData AnimationClipData;
        public int AttackDamageMultiplier; //put into modifiers?
        
#if UNITY_EDITOR
        [SerializeField, ReadOnly] public float EDITOR_finalDamage;
#endif
    }
}
