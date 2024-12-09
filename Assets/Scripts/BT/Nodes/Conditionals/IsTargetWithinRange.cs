using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Components.Combat.Interfaces;
using UnityEngine;

namespace BT.Nodes.Conditionals
{
    public class IsTargetWithinRange : Conditional
    {
        private SharedTransform _selfTransform;
        private SharedTransform _target;
        private IWeaponStatsProvider _weaponStatsProvider;
        
        public void SetSharedVariables(SharedTransform selfTransform, SharedTransform targetTransform, 
            IWeaponStatsProvider weaponStatsProvider)
        {
            _selfTransform = selfTransform;
            _target = targetTransform;
            this._weaponStatsProvider = weaponStatsProvider;
        }

        public override TaskStatus OnUpdate()
        {
            return IsWithinRange() ? TaskStatus.Success : TaskStatus.Failure;
        }

        private bool IsWithinRange()
        {
            return Vector3.Distance(_target.Value.position, _selfTransform.Value.position) <= _weaponStatsProvider.GetCombatStats().AttackRange;
        }
    }
}
