using UnityEngine;

namespace Game.Managers.RewardManager
{
	[System.Serializable]
	public class AwardItem
	{
		public AwardData data;
		[Min(1)]
		public int count;
	}
}