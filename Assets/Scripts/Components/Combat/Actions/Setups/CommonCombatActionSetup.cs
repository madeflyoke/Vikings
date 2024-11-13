using System;
using System.Collections.Generic;
using Components.Animation;
using Components.Combat.Actions.Attributes;
using Components.Combat.Actions.Conditions;
using Components.Combat.Weapons;
using Components.Combat.Weapons.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Combat.Actions.Setups
{
    [Serializable]
    public abstract class CommonCombatActionSetup
    {
        public CombatActionConditions Conditions;
        
        public AnimationClipData AnimationClipData;
        public int AttackDamageMultiplier; //put into modifiers?
        
#if UNITY_EDITOR
        [SerializeField, ReadOnly] public float EDITOR_finalDamage;
#endif
    }
    
    [CombatActionVariant(CombatActionVariant = typeof(MeleeAttack))]
    public class MeleeAttackSetup : CommonCombatActionSetup
    {
       
    }
    
    [CombatActionVariant(CombatActionVariant = typeof(ShootProjectile))]
    public class ShootProjectileSetup : CommonCombatActionSetup
    {
        
    }
}
