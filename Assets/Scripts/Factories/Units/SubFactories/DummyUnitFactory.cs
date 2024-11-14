using Components;
using Components.Settings;
using Factories.Decorators;
using Factories.Units.SubFactories.Attributes;
using Factories.Units.SubFactories.Base;
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
            
            DecorateBy(new ModelHolderDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<ModelHolderSettings>()));
            
            DecorateBy(new HealthComponentDecorator(Config.ComponentsSettingsHolder
                .GetComponentSettings<HealthComponentSettings>()));
            
            return base.CreateProduct();
        }
    }
}
