using UnityEngine;

namespace Game.Systems.LevelSystem
{
	public abstract class GoalConfig : ScriptableObject
	{
		public virtual string LocalizationTitleKey => "goal";
	
		public Information information;
	}
}