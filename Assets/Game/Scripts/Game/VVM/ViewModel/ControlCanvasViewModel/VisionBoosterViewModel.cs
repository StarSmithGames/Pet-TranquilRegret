using Game.Systems.BoosterManager;
using Game.Systems.UISystem;
using Game.UI;
using System;

namespace Game.VVM
{
	public sealed class VisionBoosterViewModel : InfinityBoosterViewModel< UIVisionBooster, VisionBooster >
	{
		private readonly UIRootGame _rootGame;
		
		public VisionBoosterViewModel(
			BoosterManager boosterManager,

			UIRootGame rootGame
			) : base(boosterManager)
		{
			_rootGame = rootGame ?? throw new ArgumentNullException( nameof(rootGame) );
		}

		public override void Initialize()
		{
			_booster = _boosterManager.VisionBooster;
			
			EnableView( true );
		}

		protected override void BoosterClicked()
		{
			_boosterManager.UseVision();
		}

		protected override UIVisionBooster GetView() => _rootGame.ControlCanvas.VisionBooster;
	}
}