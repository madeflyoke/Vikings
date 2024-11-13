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
using Components.Combat.Weapons.Enums;
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
        private readonly WeaponsConfig _weaponsConfig;

        public CombatComponentDecorator(WeaponsConfig weaponsConfig,
            HumanoidModelHolder humanoidModelHolder, CombatComponentSettings combatComponentSettings)
        {
            _combatComponentSettings = combatComponentSettings;
            _humanoidModelHolder = humanoidModelHolder;
            _weaponsConfig = weaponsConfig;
        }
        
        public IEntityComponent Decorate()
        {
            return CreateCombatComponent();
        }

        private CombatComponent CreateCombatComponent()
        {
            var actions = CreateRelatedCombatActions(out WeaponsHolder weaponsHolder);
            
            return new CombatComponent(weaponsHolder, actions);
        }

        private List<CombatAction> CreateRelatedCombatActions(out WeaponsHolder weaponsHolder)
        {
            var combatActionsSetup = _combatComponentSettings.CombatActionsSequence;

            Dictionary<CommonCombatActionSetup, Type> relatedCombatActionsTypes = combatActionsSetup.ToDictionary(x => x,
                z => z.GetType()
                    .GetCustomAttribute<CombatActionVariantAttribute>().CombatActionVariant); //reflect attributes to get related actions by setups
            
            List<CombatAction> relatedCombatActions = new List<CombatAction>();

            List<Weapon> weapons = CreateWeapons(relatedCombatActionsTypes.Keys.Select(x=>x.Conditions.WeaponType).ToList());
            
            List<WeaponSet> weaponsSets = new List<WeaponSet>();
            weaponsHolder = new WeaponsHolder(weapons, weaponsSets);
            
            foreach (var item in relatedCombatActionsTypes)
            {
                CombatAction action = (CombatAction) Activator.CreateInstance(item.Value);
                CommonCombatActionSetup setup = item.Key;
                WeaponSet weaponSet =null;
                
                switch (setup.Conditions.WeaponMode)
                {
                    case WeaponMode.SINGLE:
                        var weapon = weapons.FirstOrDefault(x => x.ValidateWeaponByConditions(setup.Conditions));
                        weaponSet = new WeaponSet(weapon);
                        break;
                    case WeaponMode.DUAL:
                        List<Weapon> dualWeapons = weapons.Where(x => x.ValidateWeaponByConditions(setup.Conditions)).Take(2).ToList();
                        if (dualWeapons.Count!=2)
                        {
                            var additionalWeapon = CreateWeapon(setup.Conditions.WeaponType);
                            weapons.Add(additionalWeapon);
                            dualWeapons.Add(additionalWeapon);
                        }
                        weaponSet = new WeaponSet(new WeaponPair(dualWeapons));
                        break;
                }
                
                weaponsSets.Add(weaponSet);
                action.Initialize(item.Key, weaponSet);
                relatedCombatActions.Add(action);
            }
            
            return relatedCombatActions;
        }
        
        private List<Weapon> CreateWeapons(List<WeaponType> weaponTypes)
        {
            return weaponTypes.Distinct().Select(CreateWeapon).ToList();
        }
        
        private Weapon CreateWeapon(WeaponType weaponType)
        {
            GameObjectComponentBuilder<Weapon> goBuilder = new ();
            
            var weaponData = _weaponsConfig.GetWeaponData(weaponType);

            var weapon = goBuilder
                .SetPrefab(weaponData.Prefab)
                .SetParent(_humanoidModelHolder.RightHandPoint)
                .WithOriginalPositionAndRotation()
                .Build();
            weapon.SetData(weaponData, _humanoidModelHolder);

            return weapon;
        }
    }
}
