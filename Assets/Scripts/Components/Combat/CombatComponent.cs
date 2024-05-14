using System;
using System.Collections.Generic;
using Components.Combat.Actions;
using Components.Combat.Interfaces;
using Components.Interfaces;

namespace Components.Combat
{
    public class CombatComponent : IEntityComponent, ICombatStatsCopyProvider
    {
        public event Action<float> IncreaseAttackSpeedEvent;
        
        public List<CombatAction> CombatActions { get; }
        
        private readonly CommonCombatStats _baseCombatStats;
        private readonly CommonCombatStats _currentCombatStats;

        public CombatComponent(List<CombatAction> actions, CommonCombatStats baseCombatStats)
        {
            CombatActions = actions;
           
            _baseCombatStats = baseCombatStats.Clone();
            _currentCombatStats = _baseCombatStats.Clone();
            
            CombatActions.ForEach(x=>x.SetCombatStatsProvider(this));
        }

        public void InitializeComponent()
        {
            SetAttackSpeed();
        }

        public void SetAttackSpeed(float multiplier = 1f)
        {
            _currentCombatStats.AttackSpeed = _baseCombatStats.AttackSpeed * multiplier;
            IncreaseAttackSpeedEvent?.Invoke(_currentCombatStats.AttackSpeed);
        }

        public CommonCombatStats GetCurrentCombatStatsCopy()
        {
            return _currentCombatStats.Clone();
        }
    }
}
