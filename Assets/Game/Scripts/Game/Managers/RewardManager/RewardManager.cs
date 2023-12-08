using Game.Systems.StorageSystem;

using Zenject;

namespace Game.Managers.RewardManager
{
	public sealed class RewardManager
	{
		[Inject] private StorageSystem storageSystem;

		public void Award(AwardItem item)
		{
			if(item.data is AwardCoinsData)
			{
				storageSystem.GameFastData.SoftCoins.Value += item.count;
			}
			else if(item.data is AwardDiamondsData)
			{
				storageSystem.GameFastData.HardDiamonds.Value += item.count;
			}
		}
	}
}