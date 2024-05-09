using System.Linq;
using Components;
using Components.Animation;
using Components.Combat;
using Components.Movement;
using Components.Settings;
using Factories.Decorators;
using Factories.Units.SubFactories.Attributes;
using Factories.Units.SubFactories.Base;
using Units.Base;
using Units.Enums;

namespace Factories.Units.SubFactories
{
    [UnitVariant(UnitVariant = UnitVariant.ARCHER)]
    public class ArcherUnitFactory : UnitSubFactory
    {
        public override UnitEntity CreateProduct()
        {
            var entityHolder = GetEntityComponent<EntityHolder>();
            
            DecorateBy(new ModelHolderDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<ModelHolderSettings>()));
            
            DecorateBy(new HealthComponentDecorator(Config.ComponentsSettingsHolder
                .GetComponentSettings<HealthComponentSettings>()));
            
            var movementComponent = DecorateBy(new NavMeshMovementComponentDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<MovementComponentSettings>())) as NavMeshMovementComponent;
            
            var combatComponent = DecorateBy(new CombatComponentDecorator(Config.ComponentsSettingsHolder
                .GetComponentSettings<CombatComponentSettings>())) as CombatComponent;
            
            var animationComponent = DecorateBy(new AnimationComponentDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<AnimationComponentSettings>())) as AnimationComponent;
            
            animationComponent.RegisterAnimationCallerMany(combatComponent.GetCombatActions().Select(x=>x.AnimationCaller));
            
            animationComponent.RegisterAnimationCaller(movementComponent.AnimationCaller);
            
            
            return base.CreateProduct();
        }
    }
}