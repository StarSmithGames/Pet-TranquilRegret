using Company.Module.Utils;

using Game.Systems.GameSystem;
using Game.Systems.LevelSystem;
using Game.UI;

using StarSmithGames.Core.StorageSystem;

using System;

using UnityEngine.SceneManagement;

using Zenject;

namespace Game.Systems.StorageSystem
{
	public class StorageSystem
	{
		public BootstrapData BootstrapData { get; private set; }

		public GameFastData GameFastData { get; }
		public GamePlayData GamePlayData { get; }
		public IntermediateData IntermediateData { get; private set; } = new();

		public StorageSystem(
			GameFastData gameFastData,
			GamePlayData gamePlayData
			)
		{
			GameFastData = gameFastData;
			GamePlayData = gamePlayData;

			BootstrapData = new()
			{
				appStartTimestampUTC = TimestampUtils.GetTimestamp(DateTime.UtcNow),
			};
		}

		public void Save()
		{
			GamePlayData.StorageData.Save();
		}

		public int GetLevelNumber()
		{
			return 0;//Storage.GameProgress.GetData().progressMainIndex + 1;
		}
	}

	public sealed class Storage : StarSmithGames.Core.StorageSystem.Storage
	{
		public StorageData<GameProgress> GameProgress { get; private set; }

		public override void Initialize()
		{
			base.Initialize();

			GameProgress = new(Database, "game_progress");
		}

		public override void Purge()
		{

		}
	}

	public class GameplayProgress
	{
		public void Reset()
		{

		}
	}

	public class IntermediateData
	{
		[Inject] public GameplayConfig GameplayConfig { get; private set; }

		public UIRootMenu RootMenu { get; set; }
		public UIRootGame RootGame { get; set; }

		public IntermediateData()
		{
			ProjectContext.Instance.Container.Inject(this);
		}
	}
}