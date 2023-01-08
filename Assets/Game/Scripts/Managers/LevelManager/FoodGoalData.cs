using UnityEngine;

namespace Game.Managers.LevelManager
{
	[CreateAssetMenu(fileName = "FoodGoalData", menuName = "Game/Levels/Food Goal")]
	public class FoodGoalData : CountableGoalData
	{
		public override string LocalizationTitleKey => base.LocalizationTitleKey + ".food";
	}
}