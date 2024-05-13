using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BT.Shared;
using CombatTargetsProviders.Interfaces;
using Extensions;
using Interfaces;
using UnityEngine;

namespace BT.Nodes.Actions
{
    public class FindClosestDamageableTarget : Action
    {
        private SharedDamageable _closestDamageable;
        private SharedTransform _closestTr;
        
        private SharedTransform _selfTransform;
        
        private Dictionary<Transform, IDamageable> _targets;
        private ICombatTargetsProvider _combatTargetsProvider;

        public FindClosestDamageableTarget Initialize(ICombatTargetsProvider combatTargetsProvider)
        {
            _combatTargetsProvider = combatTargetsProvider;
            return this;
        }
        
        public void SetSharedVariables(SharedTransform selfTransform, SharedDamageable closestDamageable, SharedTransform closestTr)
        {
            _selfTransform = selfTransform;
            _closestDamageable = closestDamageable;
            _closestTr = closestTr;
        }

        public override void OnStart()
        {
            base.OnStart();
            _targets ??= _combatTargetsProvider.GetCombatTargets()
                .ToDictionary(x=>x.TargetTr, z=>z.Damageable); //may be not spawned yet?
        }

        public override TaskStatus OnUpdate()
        {
            if (UpdateTargets())
            {
                _closestTr.Value =_selfTransform.Value.GetClosestTransform(_targets.Keys);
                _closestDamageable.Value = _targets[_closestTr.Value];
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }

        private bool UpdateTargets()
        {
            if (_targets.Count!=0)
            {
                _targets = _targets
                    .Where(pair => pair.Key != null && pair.Value.IsAlive)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            return _targets.Count != 0;
        }
    }
}
