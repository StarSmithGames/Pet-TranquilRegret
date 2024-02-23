using Game.Services;
using Game.Systems.StorageSystem;
using Game.Systems.UISystem;
using System;
using UnityEngine;
using Zenject;

namespace Game.UI
{
	public sealed class UIMenuScreen : MonoBehaviour
	{
		public TMPro.TextMeshProUGUI SoftCoinsText;
		public TMPro.TextMeshProUGUI HardDiamondsText;
		
		private StorageSystem _storageSystem;
		private UIRootMenu _uiRootMenu;

		[ Inject ]
		private void Construct(
			StorageSystem storageSystem,
			UIRootMenu uiRootMenu
			)
		{
			_storageSystem = storageSystem ?? throw new ArgumentNullException( nameof(storageSystem) );
			_uiRootMenu = uiRootMenu ?? throw new ArgumentNullException( nameof(uiRootMenu) );
			
			_storageSystem.GameFastData.SoftCoins.onChanged += CoinsChangedHandler;
			_storageSystem.GameFastData.HardDiamonds.onChanged += DiamondsChangedHandler;
			CoinsChangedHandler();
			DiamondsChangedHandler();
		}
		
		private void OnDestroy()
		{
			_storageSystem.GameFastData.SoftCoins.onChanged -= CoinsChangedHandler;
			_storageSystem.GameFastData.HardDiamonds.onChanged -= DiamondsChangedHandler;
		}

		public void OnSettingsButtonClick()
		{
			_uiRootMenu.DialogAggregator.ShowAndCreateIfNotExist< SettingsDialog >();
		}
		
		public void OnCoinsButtonClick()
		{
			
		}
		
		public void OnDiamondsButtonClick()
		{
			
		}
		
		private void CoinsChangedHandler()
		{
			SoftCoinsText.text = _storageSystem.GameFastData.SoftCoins.Value.ToString();
		}
		
		private void DiamondsChangedHandler()
		{
			HardDiamondsText.text = _storageSystem.GameFastData.HardDiamonds.Value.ToString();
		}
	}
}