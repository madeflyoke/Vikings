using System.Linq;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BT.Shared;
using Components.Combat.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace BT.Nodes.Actions
{
    public class FindTargetClosestPoint : Action
    {
        private SharedTransform _targetTr;
        private SharedDamageable _targetDamageable;
        
        private SharedVector3 _closestPoint;
        
        private SharedTransform _selfTransform;
        private SharedDamageable _selfDamageable;

        public void SetSharedVariables(SharedTransform targetTransform, SharedDamageable targetDamageable,
            SharedTransform selfTransform, SharedDamageable selfDamageable, SharedVector3 resultPoint)
        {
            _closestPoint = resultPoint;
            _targetTr = targetTransform;
            _targetDamageable = targetDamageable;
            
            _selfTransform = selfTransform;
            _selfDamageable = selfDamageable;
        }

        public override TaskStatus OnUpdate()
        {
            if (CheckForFreeSpaceAroundEnemy(out Vector3 pos))
            {
                _closestPoint.Value = pos; //_targetTr.Value.position
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
        
        private bool CheckForFreeSpaceAroundEnemy(out Vector3 finalPos)
        {
            finalPos = default;
            var _attackRange = 2f;
            var _agentRadius = 1f;
            
            var direction = (_selfTransform.Value.position-_targetTr.Value.position ).normalized;
           // var closestPoint = _originalTransform.Value.position + direction * _attackRange;
            
            var circleLength = 2 * Mathf.PI * _attackRange;
            var stepBetweenPoints = _agentRadius;
            var numberOfPoints = (int)(circleLength / stepBetweenPoints);
        
            var angleStep = 360f / numberOfPoints;

            var angleOffset = Mathf.Atan2(direction.z, direction.x);
            
            for (int i = 0; i < numberOfPoints; i++)
            {
                float angle = (i * angleStep* Mathf.Deg2Rad) +angleOffset ;
                Vector3 positionOnCircle = _targetTr.Value.position + new Vector3(Mathf.Cos(angle), _targetTr.Value.position.y,
                    Mathf.Sin(angle)) * _attackRange;
                var results = Physics.OverlapSphere(positionOnCircle, _agentRadius);

                bool isAnyOccupier = false;
                foreach (var collider in results)
                {
                    var hitReceiver = collider.GetComponentInChildren<IHitReceiver>();

                    if (hitReceiver == _selfDamageable.Value.HitReceiver ||
                        hitReceiver == _targetDamageable.Value.HitReceiver || hitReceiver == null)
                    {
                        continue;
                    }
                    isAnyOccupier = true;
                    break;
                }

                if (isAnyOccupier==false)
                {
                    finalPos = positionOnCircle;
                    return true;
                }
            }

            return false;
        }

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_closestPoint.Value,0.5f);
        }
    }
}
