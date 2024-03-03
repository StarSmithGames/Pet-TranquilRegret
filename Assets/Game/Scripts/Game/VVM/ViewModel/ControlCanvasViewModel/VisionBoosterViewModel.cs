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
			base.Initialize();
			
			EnableView( true );
		}

		protected override VisionBooster GetBooster() => _boosterManager.VisionBooster;

		protected override void UseBooster()
		{
			_boosterManager.UseVision();
		}

		protected override UIVisionBooster GetView() => _rootGame.ControlCanvas.VisionBooster;
	}
}