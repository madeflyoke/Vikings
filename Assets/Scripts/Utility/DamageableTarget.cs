using System;
using Components.Combat.Interfaces;
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
