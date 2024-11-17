using System;
using BT.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
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
