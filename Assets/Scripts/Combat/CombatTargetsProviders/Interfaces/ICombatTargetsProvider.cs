using System.Collections.Generic;
using Utility;

namespace Combat.CombatTargetsProviders.Interfaces
{
    public interface ICombatTargetsProvider
    {
        public List<DamageableTarget> GetAliveCombatTargets();
    }
}
