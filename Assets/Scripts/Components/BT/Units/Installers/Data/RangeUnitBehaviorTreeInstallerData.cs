using System.Collections.Generic;
using BT.Interfaces;
using Combat.CombatTargetsProviders.Interfaces;
using Components.Animation.Interfaces;
using Components.BT.Actions;
using Components.BT.Actions.Interfaces;
using Components.BT.Interfaces;
using Components.Combat.Interfaces;
using Components.Movement.Interfaces;

namespace Components.BT.Units.Installers.Data
{
    public struct RangeUnitBehaviorTreeInstallerData : IBehaviorTreeInstallerData
    {
        public IBehaviorTreeStarter BehaviorTreeStarter { get; set; }
        public EntityHolder EntityHolder { get; set; }
        public IAnimationPlayer AnimationPlayer;
        
        public IDamageable DamageableComponent;
        public IMovementProvider MovementProvider;
        public IEnumerable<IBehaviorAction> CombatActions;
        public IWeaponStatsProvider WeaponStatsProvider;
        public ICombatTargetsProvider CombatTargetsProvider;
        public ICombatTargetHolder CombatTargetHolder;
    }
}