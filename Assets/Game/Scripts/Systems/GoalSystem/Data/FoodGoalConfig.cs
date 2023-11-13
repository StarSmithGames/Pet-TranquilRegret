using UnityEngine;

namespace Game.Systems.GoalSystem
{
	[CreateAssetMenu(fileName = "FoodGoalData", menuName = "Game/Levels/Food Goal")]
	public class FoodGoalConfig : GoalConfig
	{
		public override string LocalizationTitleKey => base.LocalizationTitleKey + ".food";
	}
}