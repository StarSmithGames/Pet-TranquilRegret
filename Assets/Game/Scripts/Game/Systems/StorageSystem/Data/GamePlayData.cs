using Game.Systems.GameSystem;

using StarSmithGames.Core.StorageSystem;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.Systems.StorageSystem
{
	public sealed class GamePlayData
	{
		public ISaveLoadStorage<Storage> StorageData { get; private set; } = new PlayerPrefsStorageWrapper<Storage>("data");
		public Storage Storage => StorageData.GetStorage();

		private GameFastData gameFastData;
		private GameplayConfig gameplayConfig;

		public GamePlayData(
			GameFastData gameFastData,
			GameplayConfig gameplayConfig
			)
		{
			this.gameFastData = gameFastData;
			this.gameplayConfig = gameplayConfig;

			InitializeData();
		}

		private void InitializeData()
		{
			var storage = StorageData.GetStorage();

			if (gameFastData.IsFirstTime)
			{
				AddDataFirstTime(storage);
			}

			var isSupported = AddDataForOldPlayers(storage);
			var isPurged = PurgeOldKeys(storage);
			var isRegularLevelsResized = ResizeRegularLevelsData(storage);

			if (gameFastData.IsFirstTime || isSupported || isPurged || isRegularLevelsResized)
			{
				StorageData.Save();
			}
		}

		private void AddDataFirstTime(Storage storage)
		{
			storage.GameProgress.SetData(new(gameplayConfig.levels.Count));
		}

		private bool AddDataForOldPlayers(Storage storage)
		{
			var isSupported = false;

			return isSupported;
		}

		private bool PurgeOldKeys(Storage storage)
		{
			var isPurged = false;

			return isPurged;
		}

		private bool ResizeRegularLevelsData(Storage storage)
		{
			var isResized = false;

			var data = storage.GameProgress.GetData();
			int diffLevels = gameplayConfig.levels.Count - data.regularLevels.Count;

			if (diffLevels != 0)
			{
				if (diffLevels > 0)
				{
					data.AddRegularLevels(diffLevels);
					Debug.LogWarning($"[GamePlayData] GameProgress Regular Levels Changed on {diffLevels}");

					isResized = true;
				}
				else
				{
					Debug.LogError("[GamePlayData] GameProgress Regular Levels Decreased!");
				}
			}

			return isResized;
		}
	}
}