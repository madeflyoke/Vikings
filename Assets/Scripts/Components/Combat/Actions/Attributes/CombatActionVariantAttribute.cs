using System;

namespace Components.Combat.Actions.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CombatActionVariantAttribute : Attribute
    {
        public Type CombatActionVariant;
    }
}