using Game.Systems.BoosterManager;
using Game.UI;

namespace Game.VVM
{
	public abstract class InfinityBoosterViewModel< V, B > : BoosterViewModel< V, B >
		where V : UIInfinityBooster
		where B : InfinityBooster
	{
		protected InfinityBoosterViewModel(
			BoosterManager boosterManager
			) : base( boosterManager )
		{
		}
		
		protected override void SubscribeView()
		{
			base.SubscribeView();
		
			_booster.OnStarted += BoosterStartedHandler;
			_booster.OnStopped += BoosterStoppedHandler;
			_booster.OnTicked += BoosterTickedHandler;
		}

		protected override void UnSubscribeView()
		{
			base.UnSubscribeView();
			
			_booster.OnStarted -= BoosterStartedHandler;
			_booster.OnStopped -= BoosterStoppedHandler;
			_booster.OnTicked -= BoosterTickedHandler;
		}
		
		protected override void OnViewCreated()
		{
			base.OnViewCreated();
			ModelView.EnableCount( true );
			ModelView.SetCount( "9" );
		}
		
		private void BoosterStartedHandler()
		{
			ModelView.DisableCount();
			ModelView.EnableBar( true );
		}
		
		private void BoosterStoppedHandler()
		{
			ModelView.EnableBar( false );
			ModelView.EnableCount( true );
			ModelView.SetCount( "9" );
		}
		
		private void BoosterTickedHandler()
		{
			ModelView.SetFillAmountText( ((int) _booster.GetTicks() ).ToString() );
			ModelView.SetFillAmount( _booster.GetProgress() );
		}
	}
}