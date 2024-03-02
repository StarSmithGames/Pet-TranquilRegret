using Game.Systems.LevelSystem;
using System;

namespace Game.Systems.BoosterManager
{
	public sealed class BoosterManager
	{
		public VisionBooster VisionBooster { get; private set; }
		
		private readonly LevelManager _levelManager;

		public BoosterManager(
			VisionBooster visionBooster,
			LevelManager levelManager
			)
		{
			VisionBooster = visionBooster;
			
			_levelManager = levelManager ?? throw new ArgumentNullException( nameof(levelManager) );
		}
		
		public void UseVision()
		{
			if ( _levelManager.CurrentLevel is RegularLevel regularLevel )
			{
				VisionBooster.Use( regularLevel );
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}