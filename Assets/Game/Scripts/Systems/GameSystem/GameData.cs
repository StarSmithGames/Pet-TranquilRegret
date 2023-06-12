using StarSmithGames.Core.StorageSystem;

namespace Game.Systems.GameSystem
{
	public class GameData
	{
		public GameplayConfig GameplayConfig => gameplayConfig;
		private GameplayConfig gameplayConfig;

		#region Data
		public FastData<bool> IsFirstTime { get; private set; }

		public ISaveLoadStorage<Storage> StorageKeeper { get; private set; }
		#endregion

		#region NoSave
		#endregion

		public GameData(GameplayConfig gameplayConfig)
		{
			this.gameplayConfig = gameplayConfig;

			IsFirstTime = new FastData<bool>("is_first_time", true);
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