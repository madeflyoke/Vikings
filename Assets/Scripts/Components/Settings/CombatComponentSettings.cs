using System;
using System.Collections.Generic;
using Components.Combat;
using Components.Combat.Actions.Setups;
using Components.Combat.Weapons;
using Components.Combat.Weapons.Enums;
using Components.Settings.Interfaces;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Settings
{
    [Serializable]
    public class CombatComponentSettings : IComponentSettings
    {
        [OdinSerialize] public List<CommonCombatActionSetup> CombatActionsSequence;

#if UNITY_EDITOR

        [SerializeField] private WeaponsConfig EDITOR_WeaponsConfig;
        
        public void OnManualValidate()
        {
            if (CombatActionsSequence!=null && CombatActionsSequence.Count>0 && EDITOR_WeaponsConfig!=null)
            {
                CombatActionsSequence.ForEach(x=>
                {
                    if (x.Conditions != null)
                    {
                        x.EDITOR_finalDamage = x.AttackDamageMultiplier *
                                               EDITOR_WeaponsConfig.GetWeaponData(x.Conditions.WeaponType).Stats
                                                   .AttackDamage
                                               * (x.Conditions.WeaponMode == WeaponMode.DUAL ? 2 : 1);
                    }
                });
            }
        }
#endif
    }
}
