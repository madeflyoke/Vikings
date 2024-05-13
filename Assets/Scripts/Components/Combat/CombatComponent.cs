using System.Collections.Generic;
using Components.Combat.Actions;
using Components.Interfaces;
using Interfaces;
using Units.Base;
using Units.Enums;
using Utility;

namespace Components.Combat
{
    public class CombatComponent : IEntityComponent
    {
        public List<CombatAction> CombatActions { get; }

        public CombatComponent(List<CombatAction> actions)
        {
            CombatActions = actions;
            CombatActions.ForEach(x=>
            {
                x.Initialize();
            });
        }
    }
}
