using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace BT.Nodes.Actions
{
    public class StopMoving : Action
    {
        private NavMeshAgent _agent;
        
        public void Initialize(NavMeshAgent agent)
        {
            _agent = agent;
        }

        public override TaskStatus OnUpdate()
        {
            if (_agent.hasPath)
            {
                _agent.speed = 0;
              //  _agent.ResetPath();
               // _agent.isStopped = true;
            }
            return TaskStatus.Success;
        }
    }
}
