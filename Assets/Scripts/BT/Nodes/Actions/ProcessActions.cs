using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Components.BT.Actions.Interfaces;

namespace BT.Nodes.Actions
{
    public class ProcessActions : Action
    {
        protected List<IBehaviorAction> Actions;
        protected int CurrentActionIndex;

        public void Initialize(List<IBehaviorAction> actions)
        {
            Actions = actions;
        }

        public override void OnStart()
        {
            CurrentActionIndex = 0;
            Actions[CurrentActionIndex].Execute();
        }

        public override TaskStatus OnUpdate()
        {
            if (Actions[CurrentActionIndex].CurrentStatus == TaskStatus.Success)
            {
                CurrentActionIndex++;
                if (CurrentActionIndex >= Actions.Count)
                {
                    CurrentActionIndex = 0;
                }

                Actions[CurrentActionIndex].Execute();
            }

            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            Actions.ForEach(x => x.Stop());
        }
    }
}