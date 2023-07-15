using StarSmithGames.Core;
using StarSmithGames.Core.StorageSystem;

using Zenject;

namespace Game.Systems.GameSystem
{
	public class GameData
	{
		[Inject]
		public GameplayConfig GameplayConfig { get; private set; }

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
		public ISaveLoadStorage<Storage> StorageKeeper { get; private set; }
		#endregion

		#region IntermediateData
		#endregion

		#region Tutorials
		#endregion

		public GameData()
		{
			StorageKeeper = new PlayerPrefsStorageWrapper<Storage>("data");
		}

		public LevelConfig GetLevelConfig(int number)
		{
			return GameplayConfig.levels[number - 1];
		}

		public GameProgress GameProgress => StorageKeeper.GetStorage().GameProgress.GetData();
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
}