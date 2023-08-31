using UnityEngine;

namespace Game.Managers.LevelManager
{
	[CreateAssetMenu(fileName = "FoodGoalData", menuName = "Game/Levels/Food Goal")]
	public class FoodGoalConfig : CountableGoalConfig
	{
		public override string LocalizationTitleKey => base.LocalizationTitleKey + ".food";
	}
}