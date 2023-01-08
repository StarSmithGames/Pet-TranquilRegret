using UnityEngine;

namespace Game.Managers.LevelManager
{
	public abstract class GoalData : ScriptableObject
	{
		public virtual string LocalizationTitleKey => "ui.goals";
	}

	public abstract class CountableGoalData : GoalData
	{
		public Information information;
		[Min(1)]
		public int count;
	}

	public abstract class AbstractGoalData : GoalData
	{

	}
}