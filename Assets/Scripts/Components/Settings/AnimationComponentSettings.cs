using System;
using Components.Settings.Interfaces;
using UnityEngine;

namespace Components.Settings
{
    [Serializable]
    public class AnimationComponentSettings : IComponentSettings
    {
        public Avatar Avatar;
        public AnimatorOverrideController OverrideAnimatorController;
    }
}
