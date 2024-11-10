using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Builders.Utility;
using Components;
using Components.Combat;
using Components.Combat.Actions;
using Components.Combat.Actions.Attributes;
using Components.Combat.Actions.Setups;
using Components.Combat.Weapons;
using Components.Interfaces;
using Components.Settings;
using Components.View;
using Interfaces;
using UnityEngine;

namespace Factories.Decorators
{
    public class CombatComponentDecorator : IEntityDecorator
    {
        private readonly CombatComponentSettings _combatComponentSettings;
        private readonly HumanoidModelHolder _humanoidModelHolder;

        public CombatComponentDecorator(HumanoidModelHolder humanoidModelHolder, CombatComponentSettings combatComponentSettings)
        {
            _combatComponentSettings = combatComponentSettings;
            _humanoidModelHolder = humanoidModelHolder;
        }
        
        public IEntityComponent Decorate()
        {
            return CreateCombatComponent();
        }

        private CombatComponent CreateCombatComponent()
        {
            return new CombatComponent(CreateWeapon(), GetRelatedCombatActions());
        }

        private Weapon CreateWeapon()
        {
            GameObjectComponentBuilder<Weapon> goBuilder = new ();
            
            return goBuilder
                .SetPrefab(_combatComponentSettings.WeaponPrefab)
                .SetParent(_humanoidModelHolder.RightHandPoint)
                .WithOriginalPositionAndRotation()
               // .SetRotation(parent.rotation)
                .Build();
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
