using Game.Systems.BoosterManager;
using Game.Systems.UISystem;
using Game.UI;
using System;

namespace Game.VVM
{
	public sealed class SpeedUpBoosterViewModel : InfinityBoosterViewModel< UISpeedUpBooster, SpeedUpBooster >
	{
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
			base.Initialize();
			
			EnableView( true );
		}

		protected override SpeedUpBooster GetBooster() => _boosterManager.SpeedUpBooster;

		protected override void UseBooster()
		{
			_boosterManager.UseSpeedUp();
		}
		
		protected override UISpeedUpBooster GetView() => _rootGame.ControlCanvas.SpeedUpBooster;
	}
}