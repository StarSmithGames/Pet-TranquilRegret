using UnityEngine;

namespace Game.Managers.LevelManager
{
	public abstract class GoalConfig : ScriptableObject
	{
		public virtual string LocalizationTitleKey => "ui.goals";
	}

	public abstract class CountableGoalConfig : GoalConfig
	{
		public Information information;
		[Min(1)]
		public int count;
	}

	public abstract class AbstractGoalConfig : GoalConfig
	{

	}
}