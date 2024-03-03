using Game.Systems.BoosterManager.Settings;
using Game.Systems.GameSystem;
using StarSmithGames.Core.StorageSystem;
using System;
using UnityEngine;

namespace Game.Systems.StorageSystem
{
	public sealed class StorageInitializerService
	{
		private readonly GameFastData _gameFastData;
		private readonly GameplayConfig _gameplayConfig;
		private readonly BoosterSettings _boosterSettings;

		public StorageInitializerService(
			GameFastData gameFastData,
			GameplayConfig gameplayConfig,
			BoosterSettings boosterSettings
			)
		{
			_gameFastData = gameFastData ?? throw new ArgumentNullException( nameof(gameFastData) );
			_gameplayConfig = gameplayConfig ?? throw new ArgumentNullException( nameof(gameplayConfig) );
			_boosterSettings = boosterSettings ?? throw new ArgumentNullException( nameof(boosterSettings) );
		}
		
		public ISaveLoadStorage< Storage > InitializeStorage()
		{
			ISaveLoadStorage< Storage > storageData = new PlayerPrefsStorageWrapper< Storage >( "data" );
			Storage storage = storageData.GetStorage();
			
			
			if ( _gameFastData.IsFirstTime )
			{
				AddDataFirstTime( storage );
			}

			var isSupported = AddDataForOldPlayers(storage);
			var isPurged = PurgeOldKeys(storage);
			var isRegularLevelsResized = ResizeRegularLevelsData(storage);

			if ( _gameFastData.IsFirstTime || isSupported || isPurged || isRegularLevelsResized )
			{
				storageData.Save();
			}

			return storageData;
		}

		private void AddDataFirstTime( Storage storage )
		{
			storage.GameProgress.SetData( new( _gameplayConfig.levels.Count ) );
			storage.SpeedUpBooster.SetData( new() { ItemsCount = _boosterSettings.SpeedUpBoosterSettings.CountOnStart } );
			storage.VisionBooster.SetData( new() { ItemsCount = _boosterSettings.VisionBoosterSettings.CountOnStart } );
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
			int diffLevels = _gameplayConfig.levels.Count - data.regularLevels.Count;

			if (diffLevels != 0)
			{
				if (diffLevels > 0)
				{
					data.AddRegularLevels(diffLevels);
					Debug.LogWarning($"[StorageSystem] GameProgress Regular Levels Changed on {diffLevels}");

					isResized = true;
				}
				else
				{
					Debug.LogError("[StorageSystem] GameProgress Regular Levels Decreased!");
				}
			}

			return isResized;
		}
	}
}