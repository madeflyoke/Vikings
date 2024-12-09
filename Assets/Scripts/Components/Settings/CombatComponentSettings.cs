using System;
using System.Collections.Generic;
using System.Linq;
using Components.BT.Actions.Interfaces;
using Components.BT.Actions.Setups;
using Components.Combat;
using Components.Combat.Weapons;
using Components.Combat.Weapons.Enums;
using Components.Settings.Interfaces;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace Components.Settings
{
    [Serializable]
    public class CombatComponentSettings : IComponentSettings
    {
        [OdinSerialize] public List<IBehaviorActionSetup> CombatActionsSequence;

#if UNITY_EDITOR

        [SerializeField] private WeaponsConfig EDITOR_WeaponsConfig;
        
        public void OnManualValidate()
        {
            if (CombatActionsSequence!=null && CombatActionsSequence.Count>0 && EDITOR_WeaponsConfig!=null)
            {
                CombatActionsSequence.FilterCast<CommonWeaponActionSetup>().ForEach(x=>
                {
                    if (x.Conditions != null && x.Conditions.WeaponType!=WeaponType.NONE)
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
