using System;
using System.Collections.Generic;
using System.Linq;
using CombatTargetsProviders;
using CombatTargetsProviders.Interfaces;
using Factories.Units;
using Sirenix.OdinInspector;
using Units.Enums;
using UnityEngine;

public class UnitsTeamSpawnersHolder : SerializedMonoBehaviour
{
   public static UnitsTeamSpawnersHolder Instance;
   
   [SerializeField] private UnitsAbstractFactory _unitsAbstractFactory;
   [SerializeField] private List<UnitsTeamSpawner> _unitsTeamSpawners = new();
   
   //private Dictionary<Team, List<UnitEntity>> _spawnedUnitsMap = new();

   private void Awake()
   {
      if (Instance!=null)
      {
         Destroy(gameObject);
         return;
      }

      Instance = this;
   }

   private void Start()
   {
      _unitsTeamSpawners.ForEach(x=>
      {
         x.Initialize(_unitsAbstractFactory);
      });
   }

   public void SpawnAllUnits()
   {
      _unitsTeamSpawners.ForEach(x=>
      {
         x.Spawn();
      });
   }

   public ICombatTargetsProvider GetOpponentsTargetsProvider(Team team)
   {
      if (team==Team.NONE)
      {
         throw new Exception("Can't get opposite damageable targets!");
      }
      
      var oppositeTeam = team == Team.ENEMIES ? Team.ALLIES : Team.ENEMIES;

      var targetSpawner = _unitsTeamSpawners.FirstOrDefault(x => x.Team == oppositeTeam);
      
      return targetSpawner;
   }
   
#if UNITY_EDITOR

   private void OnValidate()
   {
      foreach (var value in Enum.GetValues(typeof(Team)))
      {
         var enumValue = (Team) value;
         if (enumValue!=Team.NONE)
         {
            var targetSpawner = _unitsTeamSpawners.FirstOrDefault(x => x.Team == enumValue);
            if (targetSpawner==null)
            {
               _unitsTeamSpawners.Add(new UnitsTeamSpawner(team: enumValue));
            }
         }
      }
   }

#endif
}
