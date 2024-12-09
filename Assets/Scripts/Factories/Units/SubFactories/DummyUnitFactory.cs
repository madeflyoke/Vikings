using System;
using Components;
using Components.Animation;
using Components.Animation.Interfaces;
using Components.Health;
using Components.Settings;
using Components.TagHolder;
using Components.UI;
using Components.View;
using Factories.Decorators;
using Factories.Units.SubFactories.Attributes;
using Factories.Units.SubFactories.Base;
using UniRx;
using Units.Base;
using Units.Enums;

namespace Factories.Units.SubFactories
{
    [UnitVariant(UnitVariant = UnitVariant.DUMMY)]
    public class DummyUnitFactory : UnitSubFactory
    {
        public override UnitEntity CreateProduct()
        {
            var entityHolder = Entity.GetEntityComponent<EntityHolder>();

            var modelHolder = DecorateBy(new ModelHolderDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<ModelHolderSettings>())) as ModelHolder;

            var entityInfoView =
                DecorateBy(new EntityInfoViewUIComponentDecorator(modelHolder.TopPoint)) as EntityInfoViewUI;
            var team = Entity.GetEntityComponent<UnitTagHolder>().Team;

            var healthComponent = DecorateBy(new HealthComponentDecorator(
                    Config.ComponentsSettingsHolder.GetComponentSettings<HealthComponentSettings>(),
                    modelHolder, entityInfoView.transform, TeamsConfig.GetTeamConfigData(team).RelatedColor)) as
                HealthComponent;

            DecorateBy(new NavMeshMovementComponentDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<MovementComponentSettings>()));

            return base.CreateProduct();
        }
    }
}