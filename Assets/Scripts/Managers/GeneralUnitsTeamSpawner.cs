using System;
using System.Collections.Generic;
using System.Linq;
using CombatTargetsProviders;
using CombatTargetsProviders.Interfaces;
using Factories.Units;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Units.Enums;
using UnityEditor;
using UnityEngine;
using Utility;

namespace Managers
{
   public class GeneralUnitsTeamSpawner : SerializedMonoBehaviour
   {
      public static GeneralUnitsTeamSpawner Instance;
   
      [SerializeField] private UnitsAbstractFactory _unitsAbstractFactory;
      [SerializeField] private List<UnitsTeamSpawner> _unitsTeamSpawners = new();
      private bool _spawned;
      
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
         _spawned = true;
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

      [Button]
      private void SetupSpawners()
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
      
#if UNITY_EDITOR

      [SerializeField] private bool EDITOR_drawGizmos;
      
      private void OnDrawGizmos()
      {
         if (_spawned && EDITOR_drawGizmos)
         {
            var enemies = GetOpponentsTargetsProvider(Team.ALLIES)?.GetAliveCombatTargets();
            var allies =GetOpponentsTargetsProvider(Team.ENEMIES)?.GetAliveCombatTargets();

            if (enemies.IsNullOrEmpty() || allies.IsNullOrEmpty())
            {
               return;
            }
         
            Handles.color = Color.yellow;
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.black;
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            
            foreach (var enemy in enemies)
            {
               foreach (var ally in allies)
               {
                  var enemyPos = enemy.TargetTr.position;
                  var allyPos = ally.TargetTr.position;
                  Handles.DrawLine(enemyPos, allyPos,3f);
                  Handles.Label(Vector3.Lerp(enemyPos, allyPos, .5f)+Vector3.up, 
                     Vector3.Distance(enemyPos, allyPos).ToString("F"),style);
               }
            }
         }
        
      }

#endif

#endif
   }
}
