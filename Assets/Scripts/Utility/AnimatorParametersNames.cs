using System.Collections.Generic;

namespace Utility 
{
	public static class AnimatorParametersNames
	{
		public static string IdleSpeedMultiplier = "IdleSpeedMultiplier";
		public static string MovingSpeedMultiplier = "MovingSpeedMultiplier";
		public static string CombatActionSpeedMultiplier = "CombatActionSpeedMultiplier";
		public static string CurrentVelocity = "CurrentVelocity";

		private static Dictionary<string, string> SpeedMultipliersByStateName = new()
		{
			{AnimatorStatesNames.Idle, IdleSpeedMultiplier},
			{AnimatorStatesNames.Moving, MovingSpeedMultiplier},
			{AnimatorStatesNames.Combat, CombatActionSpeedMultiplier},
		};
		
		public static string GetCorrespondingParameter(string stateName)
		{
			return SpeedMultipliersByStateName[stateName];
		}
	}
}