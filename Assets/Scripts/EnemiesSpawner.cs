using System.Collections.Generic;
using System.Linq;
using Components;
using Factories;
using Factories.Units;
using Units.Base;
using Units.Enums;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
   public static EnemiesSpawner Instance;
   
   [SerializeField] private List<UnitClass> _enemies;
   [SerializeField] private List<UnitClass> _allies;

   [SerializeField] private UnitsFactory _unitsFactory;

   [SerializeField] private List<Transform> _enemiesPoints;
   [SerializeField] private List<Transform> _alliesPoints;

   private List<EntityHolder> EnemiesSpawnedUnits { get; } = new();
   private List<EntityHolder> AlliesSpawnedUnits { get; } = new();
   
   private void Awake()
   {
      if (Instance!=null)
      {
         Destroy(this);
         return;
      }
      Instance = this;
   }

   private void Start()
   {
      for (int i = 0; i < _enemiesPoints.Count; i++)
      {
         if (_enemies.Count-1>=i)
         {
            var pointTr = _enemiesPoints[i];
            var unitClass = _enemies[i];

            EnemiesSpawnedUnits.Add(_unitsFactory
               .SetProductRequestData(unitClass, pointTr.position, pointTr.rotation, pointTr)
               .CreateProduct().GetEntityComponent<EntityHolder>());
         }
      }
      //
      // for (int i = 0; i < _alliesPoints.Count; i++)
      // {
      //    if (_allies.Count-1>=i)
      //    {
      //       var pointTr = _alliesPoints[i];
      //       var unitClass = _allies[i];
      //
      //       // AlliesSpawnedUnits.Add(_unitsFactory.CreateUnit(unitClass, pointTr.position, pointTr.rotation, pointTr));
      //    }
      // }
   }

   // private UnitHolder GetClosestEnemy(Vector3 originalPoint)
   // {
   //    return EnemiesSpawnedUnits.OrderBy(x => Vector3.Distance(originalPoint, x.transform.position)).FirstOrDefault());
   // }
}
