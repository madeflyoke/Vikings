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
        [SerializeField] private string _path;
        [SerializeField] private List<ExternalBehaviorTree> _externalBehaviorTrees;
    
        [Button]
        private void Generate()
        {
            ClassGenerator classGenerator = new ClassGenerator();

            Dictionary<string, List<string>> _tasksNamesMap = new();

            _tasksNamesMap = _externalBehaviorTrees.ToDictionary(x => x.name,
                z => z.FindTasks<Action>().Select(x => x.FriendlyName)
                    .Concat(z.FindTasks<Conditional>().Select(v=>v.FriendlyName)).ToList());
            
            var similarElements = _tasksNamesMap.Values.SelectMany(x=>x).ToList()
                .GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            
            for (int i = 0; i < _tasksNamesMap.Keys.Count; i++)
            {
                var key = _tasksNamesMap.Keys.ToList()[i];
                _tasksNamesMap[key] = _tasksNamesMap[key].Except(similarElements).ToList();
                classGenerator.GenerateStatic($"{key}TasksNames",
                        _tasksNamesMap[key], _path);
            }
            
            classGenerator.GenerateStatic("CommonBehaviorTasksNames",
                similarElements, _path);
        }
#endif
    }
}