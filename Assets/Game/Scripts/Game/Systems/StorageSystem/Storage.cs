using StarSmithGames.Core.StorageSystem;

namespace Game.Systems.StorageSystem
{
	public sealed class Storage : StarSmithGames.Core.StorageSystem.Storage
	{
		public StorageData< GameProgressData > GameProgress { get; private set; }
		
		public StorageData< SpeedUpBoosterData > SpeedUpBooster { get; private set; }
		public StorageData< VisionBoosterData > VisionBooster { get; private set; }

		public override void Initialize()
		{
			base.Initialize();

			GameProgress = new(Database, "game_progress");

			SpeedUpBooster = new( Database, "booster_speed_up" );
			VisionBooster = new( Database, "booster_vision" );
		}

		public override void Purge()
		{

		}
	}
}