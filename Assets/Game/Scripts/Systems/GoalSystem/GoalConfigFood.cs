using UnityEngine;

namespace Game.Systems.GoalSystem
{
	public class GoalConfigFood : GoalConfig
	{
		public override string LocalizationTitleKey => base.LocalizationTitleKey + ".food";
	}
}