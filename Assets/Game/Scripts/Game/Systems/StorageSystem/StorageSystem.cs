using StarSmithGames.Core.StorageSystem;
using StarSmithGames.Core.Utils;

using System;

namespace Game.Systems.StorageSystem
{
	public sealed class StorageSystem
	{
		public BootstrapData BootstrapData { get; private set; }

		public GameFastData GameFastData { get; }
		
		public Storage Storage => StorageData.GetStorage();
		
		private readonly ISaveLoadStorage< Storage > StorageData;

		public StorageSystem(
			GameFastData gameFastData,
			StorageInitializerService storageInitializerService
			)
		{
			GameFastData = gameFastData;
			StorageData = storageInitializerService.InitializeStorage();

			BootstrapData = new()
			{
				appStartTimestampUTC = TimestampUtils.GetTimestamp(DateTime.UtcNow),
			};
		}

		public void Save()
		{
			StorageData.Save();
		}

		public int GetLevelNumber()
		{
			return 0;//Storage.GameProgress.GetData().progressMainIndex + 1;
		}
	}

	public class GameplayProgress
	{
		public void Reset()
		{

		}
	}
}