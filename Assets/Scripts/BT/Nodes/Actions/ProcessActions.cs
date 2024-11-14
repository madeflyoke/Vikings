using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BT.Interfaces;
using UnityEngine;

namespace BT.Nodes.Actions
{
    public class ProcessActions : Action
    {
        private List<IBehaviorAction> _actions;
        private int _currentActionIndex;
        
        public void Initialize(List<IBehaviorAction> actions)
        {
            _actions = actions;
        }

        public override void OnStart()
        {
            _currentActionIndex = 0;
            _actions[_currentActionIndex].Execute();
        }

        public override TaskStatus OnUpdate()
        {
            if (_actions[_currentActionIndex].GetCurrentStatus()==TaskStatus.Success)
            {
                _currentActionIndex++;
                if (_currentActionIndex >= _actions.Count)
                {
                    return TaskStatus.Success;
                }
                _actions[_currentActionIndex].Execute();
            }
            return TaskStatus.Running;
        }

    }
}
