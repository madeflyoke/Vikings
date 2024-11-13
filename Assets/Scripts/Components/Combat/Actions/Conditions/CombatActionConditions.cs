using System;
using System.Collections.Generic;
using Components.Combat.Weapons.Enums;
using Components.Combat.Weapons.Interfaces;
using Sirenix.Serialization;

namespace Components.Combat.Actions.Conditions
{
    [Serializable]
    public class CombatActionConditions
    {
        public WeaponType WeaponType;
        public WeaponMode WeaponMode;
        [OdinSerialize] public List<IWeaponAttackHandler> AttackHandlers;
    }
}
