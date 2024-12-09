using System.Collections.Generic;

namespace Utility 
{
	public static class AnimatorParametersNames
	{
		public static string IdleSpeedMultiplier = "IdleSpeedMultiplier";
		public static string MovingSpeedMultiplier = "MovingSpeedMultiplier";
		public static string CombatActionSpeedMultiplier = "CombatActionSpeedMultiplier";

		private static Dictionary<string, string> SpeedMultipliersByStateName = new()
		{
			{AnimationStatesNames.Idle, IdleSpeedMultiplier},
			{AnimationStatesNames.Moving, MovingSpeedMultiplier},
			{AnimationStatesNames.Combat, CombatActionSpeedMultiplier},
		};
		
		public static string GetCorrespondingParameter(string stateName)
		{
			return SpeedMultipliersByStateName[stateName];
		}
	}
}