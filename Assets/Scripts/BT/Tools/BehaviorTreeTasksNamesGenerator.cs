using System.Linq;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using Tools;
using UnityEngine;

namespace BT.Tools
{
    public class BehaviorTreeTasksNamesGenerator : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private string _path;
        [SerializeField] private ExternalBehaviorTree _behaviorTree;
    
        [Button, UnityEngine.Tooltip("Play Mode only")]
        private void Generate()
        {
            ClassGenerator classGenerator = new ClassGenerator();

            var actionTasksNames = _behaviorTree.FindTasks<Action>().Select(x => x.FriendlyName);
            var conditionalTasksNames = _behaviorTree.FindTasks<Conditional>().Select(x => x.FriendlyName);

            var finalNames = actionTasksNames.Concat(conditionalTasksNames).ToList();
        
            classGenerator.GenerateStatic(_behaviorTree.name + "TasksNames",
                finalNames, _path);
        }
#endif
    }
}