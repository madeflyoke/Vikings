using System.Collections.Generic;
using CombatTargetsProviders.Interfaces;
using Components.BT.Interfaces;
using Components.Combat.Actions;
using Components.Combat.Interfaces;
using Managers.Interfaces;
using UnityEngine.AI;

namespace Components.BT.Units.Installers.Data
{
    public struct MeleeUnitBehaviorTreeInstallerData : IBehaviorTreeInstallerData
    {
        public IBehaviorTreeStarter BehaviorTreeStarter { get; set; }
        public EntityHolder EntityHolder { get; set; }
        
        public NavMeshAgent Agent;
        public IEnumerable<CombatAction> CombatActions;
        public ICombatStatsCopyProvider CombatStatsCopyProvider;
        public ICombatTargetsProvider CombatTargetsProvider;
        public ICombatTargetHolder CombatTargetHolder;
    }
}