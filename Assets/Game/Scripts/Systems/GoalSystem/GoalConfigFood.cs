using UnityEngine;

namespace Game.Systems.GoalSystem
{
	[CreateAssetMenu(fileName = "GoalConfigFood", menuName = "Game/Levels/Food Goal")]
	public class GoalConfigFood : GoalConfig
	{
		public override string LocalizationTitleKey => base.LocalizationTitleKey + ".food";
	}
}