using System;
using System.Reflection;
using Components.Animation.Interfaces;
using Components.BT.Actions.Attributes;
using Components.BT.Actions.Containers;
using Components.BT.Actions.Containers.Interfaces;
using Components.BT.Actions.Interfaces;
using Components.BT.Actions.Setups;
using Components.Combat.Weapons;
using Factories.Interfaces;

namespace Components.BT.Actions.Factories
{
    public class CombatBehaviorActionsFactory : IFactory<IBehaviorAction>
    {
        private readonly CombatBehaviorActionContainer _container =new();
        
        public CombatBehaviorActionsFactory(WeaponsHolder weaponsHolder=null, IAnimationPlayer animationPlayer=null)
        {
            _container.AnimationPlayer = animationPlayer;
            _container.WeaponsHolder = weaponsHolder;
        }

        public CombatBehaviorActionsFactory SetSpecifications(IBehaviorActionSetup targetActionSetup)
        {
            _container.TargetActionSetup = targetActionSetup;
            return this;
        }

        public IBehaviorAction CreateProduct()
        {
            return CreateRelatedAction();
        }

        private IBehaviorAction CreateRelatedAction()
        {
            Type targetType = _container.TargetActionSetup.GetType().GetCustomAttribute<BehaviorActionVariantAttribute>()
                .ActionVariant;
            
            var action = (IBehaviorAction) Activator.CreateInstance(targetType);
            if (action is IBehaviorActionInjectedByContainer value)
            {
                value.Construct(_container);
            }

            return action;
        }
    }
}