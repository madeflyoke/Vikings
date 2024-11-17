using System.Collections.Generic;
using BT.Interfaces;
using Combat.CombatTargetsProviders.Interfaces;
using Components.Animation;
using Components.Animation.Interfaces;
using Components.BT.Interfaces;
using Components.Combat.Actions;
using Components.Combat.Interfaces;
using Interfaces;
using UnityEngine.AI;

namespace Components.BT.Units.Installers.Data
{
    public struct MeleeUnitBehaviorTreeInstallerData : IBehaviorTreeInstallerData
    {
        public IBehaviorTreeStarter BehaviorTreeStarter { get; set; }
        public EntityHolder EntityHolder { get; set; }
        public IAnimationCallerRegister AnimationsRegister;
        public IDamageable DamageableComponent;
        
        public NavMeshAgent Agent;
        public IEnumerable<CombatAction> CombatActions;
        public ICombatStatsProvider CombatStatsProvider;
        public ICombatTargetsProvider CombatTargetsProvider;
        public ICombatTargetHolder CombatTargetHolder;
    }
}