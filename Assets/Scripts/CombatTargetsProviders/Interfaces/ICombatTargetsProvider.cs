using System.Collections.Generic;
using Utility;

namespace CombatTargetsProviders.Interfaces
{
    public interface ICombatTargetsProvider
    {
        public List<DamageableTarget> GetCombatTargets();
    }
}
