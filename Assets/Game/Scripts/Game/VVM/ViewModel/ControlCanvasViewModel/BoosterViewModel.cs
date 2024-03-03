using Game.Systems.BoosterManager;
using Game.Systems.StorageSystem;
using Game.UI;
using System;
using UnityEngine;

namespace Game.VVM
{
	public abstract class BoosterViewModel< V, B > : ViewModel< V >
		where V : UIBooster
		where B : Booster
	{
		protected B _booster;
		protected BoosterData _boosterData;
		
		protected readonly BoosterManager _boosterManager;

		public BoosterViewModel(
			BoosterManager boosterManager
			)
		{
			_boosterManager = boosterManager ?? throw new ArgumentNullException( nameof(boosterManager) );
		}

		public override void Initialize()
		{
			_booster = GetBooster();
			_boosterData = _booster.Data;

			_boosterData.onChanged += OnBoosterChanged;
		}

		public override void Dispose()
		{
			Debug.LogError( "Dispose" );
			_boosterData.onChanged -= OnBoosterChanged;

			base.Dispose();
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
			ModelView.EnableCount( !_boosterData.IsEmpty );
			ModelView.SetCount( _boosterData.ItemsCount.ToString() );
		}

		protected virtual void OnBoosterChanged()
		{
			ModelView.SetCount( _boosterData.ItemsCount.ToString() );
		}

		protected virtual void BoosterClicked()
		{
			if ( _boosterData.IsEmpty )
			{
				Debug.LogError( "Empty" );
			}
			else
			{
				UseBooster();
			}
		}

		protected abstract B GetBooster();
		
		protected abstract void UseBooster();
	}
}