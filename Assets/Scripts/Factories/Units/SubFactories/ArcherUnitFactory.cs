using System.Linq;
using Components;
using Components.Animation;
using Components.Combat;
using Components.Health;
using Components.Movement;
using Components.Settings;
using Components.TagHolder;
using Components.UI;
using Components.View;
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
            var entityHolder = Entity.GetEntityComponent<EntityHolder>();
            
            var modelHolder =DecorateBy(new ModelHolderDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<ModelHolderSettings>())) as ModelHolder;

            var entityInfoView = DecorateBy(new EntityInfoViewUIComponentDecorator(modelHolder.TopPoint)) as EntityInfoViewUI;
            var team = Entity.GetEntityComponent<UnitTagHolder>().Team;
            
            var healthComponent =DecorateBy(new HealthComponentDecorator(Config.ComponentsSettingsHolder.GetComponentSettings<HealthComponentSettings>(), 
                modelHolder, entityInfoView.transform, TeamsConfig.GetTeamConfigData(team).RelatedColor)) as HealthComponent;
            
            var navMeshMovementComponent = DecorateBy(new NavMeshMovementComponentDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<MovementComponentSettings>())) as NavMeshMovementComponent;
            
            var combatComponent = DecorateBy(new CombatComponentDecorator(WeaponsConfig,Entity.GetEntityComponent<HumanoidModelHolder>(),
                Config.ComponentsSettingsHolder
                    .GetComponentSettings<CombatComponentSettings>())) as CombatComponent;
            
            var animationComponent = DecorateBy(new AnimationComponentDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<AnimationComponentSettings>())) as AnimationComponent;
            
            animationComponent.RegisterAnimationCallerMany(combatComponent.CombatActions);
            
            animationComponent.RegisterAnimationCaller(navMeshMovementComponent);

            return base.CreateProduct();
        }
    }
}