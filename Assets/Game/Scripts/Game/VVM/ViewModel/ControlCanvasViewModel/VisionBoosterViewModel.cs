using Game.Systems.BoosterManager;
using Game.Systems.UISystem;
using Game.UI;
using System;

namespace Game.VVM
{
	public sealed class VisionBoosterViewModel : ViewModel< UIVisionBooster >
	{
		private VisionBooster _booster;
		
		private readonly UIRootGame _rootGame;
		private readonly BoosterManager _boosterManager;
		
		public VisionBoosterViewModel(
			UIRootGame rootGame,
			BoosterManager boosterManager
			)
		{
			_rootGame = rootGame ?? throw new ArgumentNullException( nameof(rootGame) );
			_boosterManager = boosterManager ?? throw new ArgumentNullException( nameof(boosterManager) );
		}

		public override void Initialize()
		{
			_booster = _boosterManager.VisionBooster;
			
			EnableView( true );
		}

		protected override void SubscribeView()
		{
			base.SubscribeView();
		
			ModelView.OnButtonClicked += VisionBoosterClicked;

			_booster.OnStarted += VisionBoosterStartedHandler;
			_booster.OnStopped += VisionBoosterStoppedHandler;
			_booster.OnTicked += VisionBoosterTickedHandler;
		}

		protected override void UnSubscribeView()
		{
			base.UnSubscribeView();
			
			ModelView.OnButtonClicked -= VisionBoosterClicked;
			
			_booster.OnStarted -= VisionBoosterStartedHandler;
			_booster.OnStopped -= VisionBoosterStoppedHandler;
			_booster.OnTicked -= VisionBoosterTickedHandler;
		}
		
		protected override void OnViewCreated()
		{
			ModelView.EnableBar( false );
			ModelView.EnableCount( true );
			ModelView.SetCount( "9" );
		}
		
		private void VisionBoosterStartedHandler()
		{
			ModelView.DisableCount();
			ModelView.EnableBar( true );
		}
		
		private void VisionBoosterStoppedHandler()
		{
			ModelView.EnableBar( false );
			ModelView.EnableCount( true );
			ModelView.SetCount( "9" );
		}
		
		private void VisionBoosterTickedHandler()
		{
			ModelView.SetFillAmountText( ((int) _booster.GetTicks() ).ToString() );
			ModelView.SetFillAmount( _booster.GetProgress() );
		}
		
		private void VisionBoosterClicked()
		{
			_boosterManager.UseVision();
		}

		protected override UIVisionBooster GetView() => _rootGame.ControlCanvas.VisionBooster;
	}
}