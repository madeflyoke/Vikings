using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private List<Vector3> points;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _agentRadius;
    
    [Button]
    public void FindPoint()
    {
        points = new();
        var circleLength = 2 * Mathf.PI * _attackRange;
        var stepBetweenPoints = _agentRadius + _agentRadius/2f + 0.5f;
        var numberOfPoints = (int)(circleLength / stepBetweenPoints);
        
        float angleStep = 360f / numberOfPoints;

        for (int i = 0; i < numberOfPoints; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 position = transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * _attackRange;

            points.Add(position);
        }
    }

    [Button]
    public void Check()
    {
        var results = Physics.OverlapSphere(transform.position + transform.forward*2f, _agentRadius);
        points = results.Select(x=>x.transform.position).ToList();

        foreach (var VARIABLE in results)
        {
            Debug.LogWarning(VARIABLE);
        }
        
    }
    
    
    private void OnDrawGizmos()
    {
        if (points==null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + transform.forward*2f, _agentRadius);
        // foreach (var point in points)
        // {
        //     Gizmos.DrawSphere(point, 1f);
        // }
    }
}
