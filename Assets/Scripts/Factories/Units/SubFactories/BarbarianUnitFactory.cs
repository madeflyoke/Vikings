using System.Linq;
using Components;
using Components.Animation;
using Components.BT.Units.Installers;
using Components.BT.Units.Installers.Data;
using Components.Combat;
using Components.Movement;
using Components.Settings;
using Components.TagHolder;
using Components.View;
using Factories.Decorators;
using Factories.Units.SubFactories.Attributes;
using Factories.Units.SubFactories.Base;
using Managers;
using Units.Base;
using Units.Enums;
using Utility;

namespace Factories.Units.SubFactories
{
    [UnitVariant(UnitVariant = UnitVariant.BARBARIAN)]
    public class BarbarianUnitFactory : UnitSubFactory
    {
        public override UnitEntity CreateProduct()
        {
            var entityHolder = Entity.GetEntityComponent<EntityHolder>();
            
            DecorateBy(new ModelHolderDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<ModelHolderSettings>()));
            
            DecorateBy(new HealthComponentDecorator(Config.ComponentsSettingsHolder
                .GetComponentSettings<HealthComponentSettings>()));
            
            var navMeshMovementComponent = DecorateBy(new NavMeshMovementComponentDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<MovementComponentSettings>())) as NavMeshMovementComponent;
            
            var combatComponent = DecorateBy(new CombatComponentDecorator(WeaponsConfig,Entity.GetEntityComponent<HumanoidModelHolder>(),
                Config.ComponentsSettingsHolder
                .GetComponentSettings<CombatComponentSettings>())) as CombatComponent;
            
            var animationComponent = DecorateBy(new AnimationComponentDecorator(entityHolder, Config.ComponentsSettingsHolder
                .GetComponentSettings<AnimationComponentSettings>())) as AnimationComponent;
            
            animationComponent.RegisterAnimationCallerMany(combatComponent.CombatActions);
            animationComponent.RegisterAnimationCaller(navMeshMovementComponent);
            animationComponent.RegisterAnimationCaller(combatComponent);

            MeleeUnitBehaviorTreeInstallerData behaviorInstallerData = new MeleeUnitBehaviorTreeInstallerData
            {
                BehaviorTreeStarter = BattleController.Instance,
                EntityHolder = entityHolder,
                Agent = navMeshMovementComponent.Agent,
                CombatActions = combatComponent.CombatActions,
                CombatStatsProvider = combatComponent,
                CombatTargetsProvider = GeneralUnitsTeamSpawner.Instance.GetOpponentsTargetsProvider(Entity.GetEntityComponent<UnitTagHolder>().Team),
                CombatTargetHolder = combatComponent
            };
            
            var behaviorDecorator = new BehaviorTreeComponentDecorator<MeleeUnitBehaviorTreeInstaller>(
                Config.ComponentsSettingsHolder.GetComponentSettings<BehaviorTreeComponentSettings>(), behaviorInstallerData);

            DecorateBy(behaviorDecorator);
            
            return base.CreateProduct();
        }
    }
}
