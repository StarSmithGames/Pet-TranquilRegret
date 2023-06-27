using StarSmithGames.Core;
using StarSmithGames.Core.StorageSystem;

namespace Game.Systems.GameSystem
{
	public class GameData
	{
		public GameplayConfig GameplayConfig => gameplayConfig;
		private GameplayConfig gameplayConfig;

		#region FastData
		public bool IsFirstTime
		{
			get => InputOutput.PlayerPrefsGet("is_first_time").CastObject<bool>(true);
			set => InputOutput.PlayerPrefsSet("is_first_time", value);
		}

		#endregion

		#region Data

		public ISaveLoadStorage<Storage> StorageKeeper { get; private set; }
		#endregion

		#region IntermediateData
		#endregion

		#region Tutorials
		#endregion

		public GameData(GameplayConfig gameplayConfig)
		{
			this.gameplayConfig = gameplayConfig;

			StorageKeeper = new PlayerPrefsStorageWrapper<Storage>("data");
		}

		public LevelConfig GetLevelConfig(int index)
		{
			return gameplayConfig.levels[index];
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