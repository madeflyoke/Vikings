using BehaviorDesigner.Runtime.Tasks;
using Components.Animation;
using Components.Animation.Interfaces;
using UnityEngine.AI;
using Utility;

namespace BT.Nodes.Actions
{
    public class SetDeath : Action, IAnimationCallerHolder
    {
        public AnimationCaller AnimationCaller { get; private set; }
        private NavMeshAgent _agent;
        
        public SetDeath Initialize(NavMeshAgent agent)
        {
            AnimationCaller = new AnimationCaller();
            _agent = agent;
            return this;
        }
        
        public override void OnStart()
        {
            AnimationCaller.CallOnAnimationWithCallback?.Invoke(AnimationCaller,new AnimationClipData(targetStateName:AnimatorStatesNames.Death),null);
            _agent.isStopped = true;
            _agent.enabled = false;
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }
    }
}
