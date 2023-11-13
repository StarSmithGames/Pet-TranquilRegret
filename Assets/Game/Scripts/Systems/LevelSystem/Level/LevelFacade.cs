using Game.Managers.GameManager;
using Game.Systems.GameSystem;
using Game.Systems.StorageSystem;

using Zenject;

namespace Game.Systems.LevelSystem
{
	public class LevelFacade
	{
		public LevelModel Model { get; private set; }

		[Inject] private GameLoader gameLoader;
		[Inject] private GameData gameData;
		[Inject] private GameManager gameManager;
		[Inject] private SpawnSystem.SpawnSystem spawnSystem;

		public LevelFacade(LevelConfig config)
		{
			ProjectContext.Instance.Container.Inject(this);

			Model = new(config);
		}

		public void Dispose()
		{

		}

		public void Start()
		{
			spawnSystem.SpawnPlayer();
		}

		public void Complete()
		{
			var data = gameData.Storage.GameProgress.GetData();
			data.progressMainIndex++;

			Dispose();
			gameLoader.LoadMenu();
		}

		public void Lose()
		{
			Dispose();
			gameLoader.LoadMenu();
		}

		public void Leave()
		{
			Dispose();
			gameLoader.LoadMenu();
		}
	}
}