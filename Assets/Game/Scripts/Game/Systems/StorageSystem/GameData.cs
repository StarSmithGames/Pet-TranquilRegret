using Game.Systems.GameSystem;
using Game.Systems.LevelSystem;
using Game.UI;

using StarSmithGames.Core;
using StarSmithGames.Core.StorageSystem;

using UnityEngine;
using UnityEngine.Playables;
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

		public int LanguageIndex
		{
			get => InputOutput.PlayerPrefsGet("language").CastObject<int>(0);
			set => InputOutput.PlayerPrefsSet("language", value);
		}

		public FastData<PreferencesData> PreferencesParams { get; private set; } = new("player_preferences");
		#endregion

		#region Data
		public ISaveLoadStorage<Storage> StorageData { get; private set; } = new PlayerPrefsStorageWrapper<Storage>("data");
		public Storage Storage => StorageData.GetStorage();
		#endregion

		public IntermediateData IntermediateData { get; private set; } = new();

		#region Tutorials
		#endregion

		public GameData()
		{
			var storage = StorageData.GetStorage();
			if (IsFirstTime)
			{
				storage.GameProgress.SetData(new()
				{
					progressMainIndex = 0,
				});
			}
			else
			{

			}

			StorageData.Save();
		}

		public void Save()
		{
			StorageData.Save();
		}

		public int GetLevelNumber()
		{
			return Storage.GameProgress.GetData().progressMainIndex + 1;
		}
	}

	public class Storage : StarSmithGames.Core.StorageSystem.Storage
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
		[Inject] public GameplayConfig GameplayConfig { get; private set; }

		public UIRootMenu RootMenu { get; set; }
		public UIRootGame RootGame { get; set; }

		public LevelPresenter LevelPresenter { get; set; }
		public LevelConfig CurrentLevelConfig => LevelPresenter?.Model.Config;

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