using Game.Systems.GameSystem;
using Game.Systems.LevelSystem;

using StarSmithGames.Core;
using StarSmithGames.Core.StorageSystem;

using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;

namespace Game.Systems.StorageSystem
{
	public class GameData
	{
		#region FastData
		public bool IsGDPRApplied
		{
			get => InputOutput.PlayerPrefsGet("is_gdpr_applied").CastObject<bool>(false);
			set => InputOutput.PlayerPrefsSet("is_gdpr_applied", value);
		}

		public bool IsFirstTime
		{
			get => InputOutput.PlayerPrefsGet("is_first_time").CastObject<bool>(true);
			set => InputOutput.PlayerPrefsSet("is_first_time", value);
		}

		public int TimeInGame
		{
			get => InputOutput.PlayerPrefsGet("time_in_game").CastObject<int>(0);
			set => InputOutput.PlayerPrefsSet("time_in_game", value);
		}

		public int SessionsCount
		{
			get => InputOutput.PlayerPrefsGet("sessions_count").CastObject<int>(0);
			set => InputOutput.PlayerPrefsSet("sessions_count", value);
		}

		public int LevelNumber
		{
			get => InputOutput.PlayerPrefsGet("level_number").CastObject<int>(1);
			set => InputOutput.PlayerPrefsSet("level_number", value);
		}

		public int LanguageIndex
		{
			get => InputOutput.PlayerPrefsGet("language").CastObject<int>(0);
			set => InputOutput.PlayerPrefsSet("language", value);
		}
		#endregion

		#region Data
		public ISaveLoadStorage<Storage> StorageData { get; private set; } = new PlayerPrefsStorageWrapper<Storage>("data");

		public GameProgress GameProgressData => StorageData.GetStorage().GameProgress.GetData();
		public GameplayProgress GameplayProgress;
		#endregion

		public IntermediateData IntermediateData { get; private set; } = new IntermediateData();

		#region Tutorials
		#endregion
	}

	public class Storage : StarSmithGames.Core.StorageSystem.Storage
	{
		public StorageData<GameProgress> GameProgress { get; private set; }

		public override void Initialize()
		{
			base.Initialize();

			GameProgress = new StorageData<GameProgress>(Database, "game_progress");
		}

		public override void Purge()
		{

		}
	}

	public class GameProgress
	{
		public int progressMainIndex;
	}

	public class GameplayProgress
	{
		public void Reset()
		{

		}
	}

	public class IntermediateData
	{
		[Inject]
		public GameplayConfig GameplayConfig { get; private set; }

		public Level Level { get; set; }
		public LevelConfig CurrentLevelConfig => Level?.config;

		public IntermediateData()
		{
			ProjectContext.Instance.Container.Inject(this);
		}

		public LevelConfig GetLevelConfig(Scene scene)
		{
			return GameplayConfig.levels.Find((x) => x.scene.SceneName == scene.name);
		}

		public LevelConfig GetLevelConfig(int number)
		{
			return GameplayConfig.levels[number - 1];
		}
	}
}