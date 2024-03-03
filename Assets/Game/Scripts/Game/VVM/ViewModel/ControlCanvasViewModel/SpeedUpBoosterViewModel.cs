using Game.Systems.BoosterManager;
using Game.Systems.UISystem;
using Game.UI;
using System;

namespace Game.VVM
{
	public sealed class SpeedUpBoosterViewModel : BoosterViewModel< UISpeedUpBooster, SpeedUpBooster >
	{
		private SpeedUpBooster _booster;
		
		private readonly UIRootGame _rootGame;
		
		public SpeedUpBoosterViewModel(
			BoosterManager boosterManager,

			UIRootGame rootGame
			) : base(boosterManager)
		{
			_rootGame = rootGame ?? throw new ArgumentNullException( nameof(rootGame) );
		}
		
		public override void Initialize()
		{
			_booster = _boosterManager.SpeedUpBooster;
			
			EnableView( true );
		}
		
		protected override void BoosterClicked()
		{
			_boosterManager.UseSpeedUp();
		}
		
		protected override UISpeedUpBooster GetView() => _rootGame.ControlCanvas.SpeedUpBooster;
	}
}