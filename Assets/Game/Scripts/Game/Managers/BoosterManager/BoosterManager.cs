using Game.Systems.LevelSystem;
using System;

namespace Game.Systems.BoosterManager
{
	public sealed class BoosterManager
	{
		public SpeedUpBooster SpeedUpBooster { get; private set; }
		public VisionBooster VisionBooster { get; private set; }

		private readonly LevelManager _levelManager;

		public BoosterManager(
			SpeedUpBooster speedUpBooster,
			VisionBooster visionBooster,
			LevelManager levelManager
			)
		{
			SpeedUpBooster = speedUpBooster;
			VisionBooster = visionBooster;
			
			_levelManager = levelManager ?? throw new ArgumentNullException( nameof(levelManager) );
		}
		
		public void UseSpeedUp()
		{
			SpeedUpBooster.Use();
		}
		
		public void UseVision()
		{
			VisionBooster.Use();
		}
	}
}