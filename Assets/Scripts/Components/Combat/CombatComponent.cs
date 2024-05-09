using System.Collections.Generic;
using Components.Combat.Actions;
using Components.Interfaces;

namespace Components.Combat
{
    public class CombatComponent : IEntityComponent
    {
        private readonly List<CombatAction> _combatActions;

        public CombatComponent(List<CombatAction> actions)
        {
            _combatActions = actions;
            _combatActions.ForEach(x=>
            {
                x.Initialize();
            });
        }
        
        public List<CombatAction> GetCombatActions() => _combatActions;

    }
}
