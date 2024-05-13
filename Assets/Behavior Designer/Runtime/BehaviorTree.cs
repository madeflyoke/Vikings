using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    // Wrapper for the Behavior class
    [AddComponentMenu("Behavior Designer/Behavior Tree")]
    public class BehaviorTree : Behavior
    {
        // intentionally left blank
        
        public TTask FindTask<TTask>(string taskName) where TTask: Task
        {
            return FindTaskWithName(taskName) as TTask;
        }
        
    }
}