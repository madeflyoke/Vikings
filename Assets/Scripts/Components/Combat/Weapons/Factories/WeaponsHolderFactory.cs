using System.Collections.Generic;
using System.Linq;
using Builders.Utility;
using Components.BT.Actions.Setups;
using Components.Combat.Weapons.Enums;
using Components.View;
using Factories.Interfaces;

namespace Components.Combat.Weapons.Factories
{
    public class WeaponsHolderFactory : IFactory<WeaponsHolder>
    {
        private readonly WeaponsConfig _weaponsConfig;
        private readonly List<CommonWeaponActionSetup> _setups;
        private readonly HumanoidModelHolder _holder;
        
        public WeaponsHolderFactory(WeaponsConfig weaponsConfig, HumanoidModelHolder holder, List<CommonWeaponActionSetup> setups)
        {
            _setups = setups;
            _holder = holder;
            _weaponsConfig = weaponsConfig;
        }
        
        public WeaponsHolder CreateProduct()
        {
            return HandleWeapons(_setups);
        }
        
        private WeaponsHolder HandleWeapons(List<CommonWeaponActionSetup> setups)
        {
            List<Weapon> weapons = CreateWeapons(setups.Select(x => x.Conditions.WeaponType).ToList());
            List<WeaponSet> weaponsSets = new List<WeaponSet>();
            var weaponsHolder = new WeaponsHolder(weapons, weaponsSets);
            
            weaponsSets.AddRange(CreateWeaponsSets(weapons, weaponsHolder, setups));
            return weaponsHolder;
        }
        
        private IEnumerable<WeaponSet> CreateWeaponsSets(List<Weapon> weapons, WeaponsHolder weaponsHolder,
            List<CommonWeaponActionSetup> setups)
        {
            List<WeaponSet> weaponsSets = new List<WeaponSet>();
            foreach (var setup in setups)
            {
                WeaponSet weaponSet =null;
                switch (setup.Conditions.WeaponMode)
                {
                    case WeaponMode.SINGLE:
                        var weapon = weapons.FirstOrDefault(x => x.ValidateWeaponByConditions(setup.Conditions));
                        weaponSet = new WeaponSet(weapon, weaponsHolder);
                        break;
                    case WeaponMode.DUAL:
                        List<Weapon> dualWeapons = weapons.Where(x => x.ValidateWeaponByConditions(setup.Conditions)).Take(2).ToList();
                        if (dualWeapons.Count!=2)
                        {
                            var additionalWeapon = CreateWeapon(setup.Conditions.WeaponType);
                            weapons.Add(additionalWeapon);
                            dualWeapons.Add(additionalWeapon);
                        }
                        weaponSet = new WeaponSet(new WeaponPair(dualWeapons),weaponsHolder);
                        break;
                }
                
                weaponsSets.Add(weaponSet);
            }

            return weaponsSets;
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
                .SetParent(_holder.RightHandPoint)
                .WithOriginalPositionAndRotation()
                .Build();
            weapon.SetData(weaponData, _holder);

            return weapon;
        }
    }
}
