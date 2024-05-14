using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;

namespace BT.Tools
{
    public class BehaviorTreeCachedVariablesHolder
    {
        private readonly Dictionary<Type, SharedVariable> _sharedVariables;

        public BehaviorTreeCachedVariablesHolder(BehaviorTree behaviorTree)
        {
            _sharedVariables = behaviorTree.GetAllVariables().ToDictionary(x => x.GetType(),z=>z);
        }
        
        public TVariable GetVariable<TVariable>() where  TVariable : SharedVariable
        {
            return (TVariable) _sharedVariables[typeof(TVariable)];
        }
    }
}
