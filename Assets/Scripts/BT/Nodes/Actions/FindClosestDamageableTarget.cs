using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BT.Shared;
using CombatTargetsProviders.Interfaces;
using Components.Combat.Interfaces;
using Extensions;
using Interfaces;
using UnityEngine;
using Utility;

namespace BT.Nodes.Actions
{
    public class FindClosestDamageableTarget : Action
    {
        private SharedDamageable _closestDamageable;
        private SharedTransform _closestTr;
        
        private SharedTransform _selfTransform;
        
        private List<DamageableTarget> _targets;
        private ICombatTargetsProvider _combatTargetsProvider;
        private ICombatTargetHolder _combatTargetHolder;
        private DamageableTarget _lastCheckedTarget;
        
        public FindClosestDamageableTarget Initialize(ICombatTargetsProvider combatTargetsProvider, ICombatTargetHolder combatTargetHolder)
        {
            _combatTargetsProvider = combatTargetsProvider;
            _combatTargetHolder = combatTargetHolder;
            return this;
        }
        
        public void SetSharedVariables(SharedTransform selfTransform, SharedDamageable closestDamageable, SharedTransform closestTr)
        {
            _selfTransform = selfTransform;
            _closestDamageable = closestDamageable;
            _closestTr = closestTr;
        }
        
        public override TaskStatus OnUpdate()
        {
            if (TryGetTargets(out DamageableTarget possibleTarget))
            {
                _lastCheckedTarget = possibleTarget;
                _combatTargetHolder.SetCombatTarget(_lastCheckedTarget); //combat component link
                
                _closestTr.Value =possibleTarget.TargetTr;
                _closestDamageable.Value = possibleTarget.Damageable;
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }

        private bool TryGetTargets(out DamageableTarget possibleTarget)
        {
            _targets = _combatTargetsProvider.GetAliveCombatTargets();
            possibleTarget = null;
            if (_targets.Count!=0)
            {
                if (_lastCheckedTarget != null && _targets.Contains(_lastCheckedTarget)) //if not validated?
                {
                    _targets.Remove(_lastCheckedTarget);
                    _lastCheckedTarget = null;
                    if (_targets.Count==0)
                    {
                        return false;
                    }
                }
                
                var closestTr = _selfTransform.Value.GetClosestTransform(_targets.Select(x => x.TargetTr));
                var target = _targets.FirstOrDefault(x => x.TargetTr == closestTr);
                possibleTarget = target; 
                return true;
            }
            return false;
        }
    }
}
