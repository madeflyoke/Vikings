using Combat;
using Components;
using Components.Animation;
using Components.BT.Units.Installers;
using Components.BT.Units.Installers.Data;
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
    [UnitVariant(UnitVariant = UnitVariant.BARBARIAN)]
    public class BarbarianUnitFactory : UnitSubFactory
    {
        public override UnitEntity CreateProduct()
        {
            var entityHolder = Entity.GetEntityComponent<EntityHolder>();
            
            var modelHolder =DecorateBy(new ModelHolderDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<ModelHolderSettings>())) as ModelHolder;

            var entityInfoView = DecorateBy(new EntityInfoViewUIComponentDecorator(modelHolder.TopPoint)) as EntityInfoViewUI;
            var team = Entity.GetEntityComponent<UnitTagHolder>().Team;
            
            var animationComponent = DecorateBy(new AnimationComponentDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<AnimationComponentSettings>())) as AnimationComponent;
            
            var healthComponent =DecorateBy(new HealthComponentDecorator(Config.ComponentsSettingsHolder.GetComponentSettings<HealthComponentSettings>(), 
                modelHolder, entityInfoView.transform, TeamsConfig.GetTeamConfigData(team).RelatedColor)) as HealthComponent;
            
            var navMeshMovementComponent = DecorateBy(new NavMeshMovementComponentDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<MovementComponentSettings>())) as NavMeshMovementComponent;
            
            var combatComponent = DecorateBy(new CombatComponentDecorator(WeaponsConfig,Entity.GetEntityComponent<HumanoidModelHolder>(),
                Config.ComponentsSettingsHolder.GetComponentSettings<CombatComponentSettings>(),
                animationComponent,animationComponent)) as CombatComponent;
            
            MeleeUnitBehaviorTreeInstallerData behaviorInstallerData = new MeleeUnitBehaviorTreeInstallerData
            {
                BehaviorTreeStarter = BattleController.Instance,
                EntityHolder = entityHolder,
                MovementProvider = navMeshMovementComponent,
                CombatActions = combatComponent.CombatActions,
                WeaponStatsProvider = combatComponent,
                CombatTargetsProvider = GeneralUnitsTeamSpawner.Instance.GetOpponentsTargetsProvider(Entity.GetEntityComponent<UnitTagHolder>().Team),
                CombatTargetHolder = combatComponent,
                DamageableComponent = healthComponent,
                AnimationPlayer = animationComponent
            };
            
            var behaviorDecorator = new BehaviorTreeComponentDecorator<MeleeUnitBehaviorTreeInstaller>(
                Config.ComponentsSettingsHolder.GetComponentSettings<BehaviorTreeComponentSettings>(), behaviorInstallerData);

            DecorateBy(behaviorDecorator);
            
            return base.CreateProduct();
        }
    }
}
