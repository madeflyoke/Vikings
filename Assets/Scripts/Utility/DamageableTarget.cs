using System;
using Interfaces;
using UnityEngine;

namespace Utility
{
    [Serializable]
    public class DamageableTarget
    {
        public IDamageable Damageable;
        public Transform TargetTr;
    }
}
