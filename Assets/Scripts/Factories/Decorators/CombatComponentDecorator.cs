using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Components.Combat;
using Components.Combat.Actions;
using Components.Combat.Actions.Attributes;
using Components.Combat.Actions.Setups;
using Components.Interfaces;
using Components.Settings;
using Interfaces;

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
            return new CombatComponent(GetRelatedCombatActions(), _combatComponentSettings.BaseCombatStats);
        }

        private List<CombatAction> GetRelatedCombatActions()
        {
            var combatActionsSetup = _combatComponentSettings.CombatActionsSequence;

            Dictionary<CommonCombatActionSetup, Type> relatedCombatActionsTypes = combatActionsSetup.ToDictionary(x => x,
                z => z.GetType()
                    .GetCustomAttribute<CombatActionVariantAttribute>().CombatActionVariant); //reflect attributes to get related actions by setups

            List<CombatAction> relatedCombatActions = new List<CombatAction>();
            
            foreach (var item in relatedCombatActionsTypes)
            {
                var action = (CombatAction) Activator.CreateInstance(item.Value);
                action.Initialize(item.Key);
                relatedCombatActions.Add(action);
            }

            return relatedCombatActions;
        }
    }
}
