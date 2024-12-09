using System.Linq;
using Components.Animation.Interfaces;
using Components.BT.Actions.Factories;
using Components.BT.Actions.Setups;
using Components.Combat;
using Components.Combat.Weapons;
using Components.Combat.Weapons.Factories;
using Components.Interfaces;
using Components.Settings;
using Components.View;
using Interfaces;
using Sirenix.Utilities;

namespace Factories.Decorators
{
    public class CombatComponentDecorator : IEntityDecorator
    {
        private readonly CombatComponentSettings _combatComponentSettings;
        private readonly HumanoidModelHolder _humanoidModelHolder;
        private readonly WeaponsConfig _weaponsConfig;
        private readonly IAnimatorValueChanger _animatorValueChanger;
        private readonly IAnimationPlayer _animationPlayer;
        
        public CombatComponentDecorator(WeaponsConfig weaponsConfig,
            HumanoidModelHolder humanoidModelHolder, CombatComponentSettings combatComponentSettings, 
            IAnimationPlayer animationPlayer, IAnimatorValueChanger animatorValueChanger)
        {
            _combatComponentSettings = combatComponentSettings;
            _humanoidModelHolder = humanoidModelHolder;
            _weaponsConfig = weaponsConfig;
            _animationPlayer = animationPlayer;
            _animatorValueChanger = animatorValueChanger;
        }
        
        public IEntityComponent Decorate()
        {
            return CreateCombatComponent();
        }

        private CombatComponent CreateCombatComponent()
        {
            var combatActionsSetupList = _combatComponentSettings.CombatActionsSequence;
            
            var weaponActionsSetups = combatActionsSetupList.FilterCast<CommonWeaponActionSetup>().ToList();
            var weaponsHolder = new WeaponsHolderFactory(_weaponsConfig, _humanoidModelHolder, weaponActionsSetups).CreateProduct();

            var actionsFactory = new CombatBehaviorActionsFactory(weaponsHolder, _animationPlayer);
            var actions = combatActionsSetupList.Select(setup => actionsFactory.SetSpecifications(setup).CreateProduct()).ToList();

            var component =new CombatComponent(weaponsHolder, actions,_animatorValueChanger);
            return component;
        }

    }
}
