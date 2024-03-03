using Game.Managers.RewardManager;
using Game.Systems.UISystem;
using Game.UI;

namespace Game.Systems.LevelSystem
{
	public sealed class LevelRegularViewModel : LevelViewModel
	{
		public override LevelView View => RegularViewView;
		public LevelRegularView RegularViewView { get; private set; }
		
		private LevelPresenter _presenter;
		
		private readonly UIRootGame _uiRootGame;
		private readonly SpawnSystem.SpawnSystem _spawnSystem;
		private readonly StorageSystem.StorageSystem _storageSystem;
		private readonly RewardManager _rewardManager;
		
		public LevelRegularViewModel(
			UIRootGame uiRootGame,
			SpawnSystem.SpawnSystem spawnSystem,
			StorageSystem.StorageSystem storageSystem,
			RewardManager rewardManager
			)
		{
			_uiRootGame = uiRootGame;
			_spawnSystem = spawnSystem;
			_storageSystem = storageSystem;
			_rewardManager = rewardManager;
		}

		public void Initialize( 
			LevelRegularView regularView,
		    LevelPresenter presenter
			)
		{
			RegularViewView = regularView;
			_presenter = presenter;
		}

		public void Start()
		{
			_uiRootGame.GameCanvas.SetLevel( _presenter );
			_spawnSystem.SpawnPlayer();
		}

		public void Complete()
		{
			var data = _storageSystem.Storage.GameProgress.GetData();
			var level = data.regularLevels[ _storageSystem.GameFastData.LastRegularLevelIndex ];
			level.completed = 1;
			level.stars = 3;
			level.timestamp = _presenter.Timer.RemainingTime;

			_presenter.Model.Config.awards.ForEach((award) =>
			{
				_rewardManager.Award(award);
			});

			_storageSystem.Save();

			_uiRootGame.DialogAggregator.ShowAndCreateIfNotExist< FinishLevelDialog >();
		}
	}
}