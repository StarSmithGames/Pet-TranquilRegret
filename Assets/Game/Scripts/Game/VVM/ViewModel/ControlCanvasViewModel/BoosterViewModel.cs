using Game.Systems.BoosterManager;
using Game.UI;
using System;

namespace Game.VVM
{
	public abstract class BoosterViewModel< V, B > : ViewModel< V >
		where V : UIBooster
		where B : Booster
	{
		protected B _booster;
		
		protected readonly BoosterManager _boosterManager;

		public BoosterViewModel(
			BoosterManager boosterManager
			)
		{
			_boosterManager = boosterManager ?? throw new ArgumentNullException( nameof(boosterManager) );
		}
		
		protected override void SubscribeView()
		{
			base.SubscribeView();
		
			ModelView.OnButtonClicked += BoosterClicked;
		}

		protected override void UnSubscribeView()
		{
			base.UnSubscribeView();
			
			ModelView.OnButtonClicked -= BoosterClicked;
		}
		
		protected override void OnViewCreated()
		{
			ModelView.EnableCount( true );
			ModelView.SetCount( "9" );
		}

		protected abstract void BoosterClicked();
	}
}