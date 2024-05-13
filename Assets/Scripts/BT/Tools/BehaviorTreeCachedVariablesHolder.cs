using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace BT.Tools
{
    [Serializable]
    public class BehaviorTreeCachedVariablesHolder
    {
        [SerializeField] private ExternalBehaviorTree _externalBehaviorTree;
        [OdinSerialize, ReadOnly] private Dictionary<Type, SharedVariable> _sharedVariables;

        public TVariable GetVariable<TVariable>() where  TVariable : SharedVariable
        {
            return (TVariable) _sharedVariables[typeof(TVariable)];
        }
        
#if UNITY_EDITOR
        
        [Button]
        private void Setup()
        {
            _sharedVariables = _externalBehaviorTree.BehaviorSource.GetAllVariables().ToDictionary(x => x.GetType(),z=>z);
        }
#endif
    }
}
