using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utility
{
    public static class AnimationStatesNames
    {
        public const string None = "None";
        public const string Idle = "Idle";
        public const string Moving = "Moving";
        public const string Combat = "CombatAction";
        public const string Death = "Death";
        
#if UNITY_EDITOR
        
        public static List<string> GetAnimatorNamesValues()
        {
            List<string> animationList = new List<string>();

            Type animationType = typeof(AnimationStatesNames);
            FieldInfo[] fields = animationType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(string))
                {
                    string animationValue = (string)field.GetValue(null);
                    animationList.Add(animationValue);
                }
            }

            return animationList;
        }
        
#endif
    }
}
