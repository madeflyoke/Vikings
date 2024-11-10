using System;
using Components.Animation;
using Components.Combat.Actions.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Combat.Actions.Setups
{
    [Serializable]
    public abstract class CommonCombatActionSetup
    {
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
