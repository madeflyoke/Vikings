using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
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
            _agent.SetDestination(_agent.transform.position);
            //_agent.ResetPath();
            //_agent.isStopped = true;
            return TaskStatus.Success;
        }
    }
}
