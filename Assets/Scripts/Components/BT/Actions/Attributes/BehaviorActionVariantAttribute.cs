using System;
using Components.BT.Actions.Interfaces;

namespace Components.BT.Actions.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BehaviorActionVariantAttribute : Attribute
    {
        public Type ActionVariant; // IBehaviorAction primary
    }
}