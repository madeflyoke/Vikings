using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tools;
using UnityEngine;

namespace BT.Tools
{
    public class BehaviorTreeTasksNamesGenerator : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private string _path = "Assets/Scripts/BT/Utility";
        [SerializeField] private string _uniquePrefix;
        [SerializeField] private ExternalBehaviorTree _externalBehaviorTree;
    
        [Button]
        private void Generate()
        {
            ClassGenerator classGenerator = new ClassGenerator();
            
            var tasksNames = _externalBehaviorTree.FindTasks<Action>().Select(x => x.FriendlyName)
                .Concat(_externalBehaviorTree.FindTasks<Conditional>().Select(v=>v.FriendlyName)).ToList();

            tasksNames = tasksNames.Distinct().ToList();
            
            // var similarElements = _tasksNames.SelectMany(x=>x).ToList()
            //     .GroupBy(x => x)
            //     .Where(g => g.Count() > 1)
            //     .Select(g => g.Key)
            //     .ToList();
            //
            // for (int i = 0; i < _tasksNames.Keys.Count; i++)
            // {
            //     var key = _tasksNames.Keys.ToList()[i];
            //     _tasksNames[key] = _tasksNames[key].Except(similarElements).ToList();
            //     classGenerator.GenerateStatic($"{_uniquePrefix}BehaviorTasksNames",
            //             _tasksNames[key], _path, true);
            // }
            
            classGenerator.GenerateStatic($"{_uniquePrefix}BehaviorTasksNames",
                tasksNames, _path,true);
        }
#endif
    }
}