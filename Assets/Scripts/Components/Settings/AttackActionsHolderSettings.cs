using System;
using System.Collections.Generic;
using BT.Interfaces;
using Components.Settings.Interfaces;
using Sirenix.Serialization;

namespace Components.Settings
{
    [Serializable]
    public class AttackActionsHolderSettings : IComponentSettings
    {
        [OdinSerialize] public List<IBehaviorAction> Actions;
    }
}
