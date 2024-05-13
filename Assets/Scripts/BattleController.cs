using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance;
    
    public event Action StartBattleEvent;
    
    [SerializeField] private UnitsTeamSpawnersHolder unitsTeamSpawnersHolder;

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
        unitsTeamSpawnersHolder.SpawnAllUnits();
    }

    [Button]
    public void StartBattle()
    {
        StartBattleEvent?.Invoke();
    }
}
