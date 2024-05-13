using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static Transform GetClosestTransform(this Transform selfTransform, IEnumerable<Transform> values)
        {
            return values.OrderBy(x => Vector3.Distance(selfTransform.position, x.transform.position)).FirstOrDefault();
        }
    }
}
