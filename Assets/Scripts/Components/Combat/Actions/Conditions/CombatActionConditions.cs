using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Components.Combat.Weapons.Enums;
using Components.Combat.Weapons.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Combat.Actions.Conditions
{
    [Serializable]
    public class CombatActionConditions
    {
        public WeaponType WeaponType;
        public WeaponMode WeaponMode;
        [OdinSerialize, ValueDropdown(nameof(GetWeaponAttackHandlerTypes))] public List<Type> AttackHandlers;
        
#if UNITY_EDITOR
        
        private List<Type> GetWeaponAttackHandlerTypes()
        {
            Type type = typeof(IWeaponAttackHandler);
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();
        
            var implementingClasses = allTypes
                .Where(t => type.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .ToList();

            return implementingClasses;
        }
        
#endif
    }
}
