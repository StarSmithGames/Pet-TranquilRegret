using UnityEngine;

namespace Game.Systems.GoalSystem
{
	public abstract class GoalConfig : ScriptableObject
	{
		public virtual string LocalizationTitleKey => "goal";
	
		public Information information;
	}
}