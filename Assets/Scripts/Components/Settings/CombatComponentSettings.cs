using System;
using System.Collections.Generic;
using Components.Combat;
using Components.Combat.Actions.Setups;
using Components.Settings.Interfaces;
using Sirenix.Serialization;

namespace Components.Settings
{
    [Serializable]
    public class CombatComponentSettings : IComponentSettings
    {
        public CommonCombatStats BaseCombatStats;
        [OdinSerialize] public List<CommonCombatActionSetup> CombatActionsSequence;

#if UNITY_EDITOR

        public void OnManualValidate()
        {
            if (CombatActionsSequence!=null && CombatActionsSequence.Count>0)
            {
                CombatActionsSequence.ForEach(x=>x.EDITOR_finalDamage=x.AttackDamageMultiplier*BaseCombatStats.AttackDamage);
            }
        }
#endif
    }
}
