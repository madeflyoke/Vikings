using Components.Combat;
using Components.Interfaces;
using Components.Settings;
using Interfaces;
using UnityEngine.EventSystems;

namespace Factories.Decorators
{
    
    public class CombatComponentDecorator : IEntityDecorator
    {
        private readonly CombatComponentSettings _combatComponentSettings;

        public CombatComponentDecorator(CombatComponentSettings combatComponentSettings)
        {
            _combatComponentSettings = combatComponentSettings;
        }
        
        public IEntityComponent Decorate()
        {
            return CreateCombatComponent();
        }

        private CombatComponent CreateCombatComponent()
        {
            return new CombatComponent(_combatComponentSettings.CombatActions);
        }
    }
}
