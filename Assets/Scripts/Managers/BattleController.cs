using System;
using Managers.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class BattleController : MonoBehaviour, IBehaviorTreeStarter
    {
        public static BattleController Instance;
    
        public event Action BehaviorTreeStartEvent;
        
        [SerializeField] private GeneralUnitsTeamSpawner generalUnitsTeamSpawner;

        private void Awake()
        {
            if (Instance!=null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        [Button]
        public void Spawn()
        {
            generalUnitsTeamSpawner.SpawnAllUnits();
        }

        [Button]
        public void StartBattle()
        {
            BehaviorTreeStartEvent?.Invoke();
        }
    }
}
