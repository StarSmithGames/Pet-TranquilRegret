using Game.Services;
using Game.Systems.StorageSystem;
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
		private ViewService _viewService;

		[ Inject ]
		private void Construct(
			StorageSystem storageSystem,
			ViewService viewService
			)
		{
			_storageSystem = storageSystem ?? throw new ArgumentNullException( nameof(storageSystem) );
			_viewService = viewService ?? throw new ArgumentNullException( nameof(viewService) );
			
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
			_viewService.TryShowDialog< SettingsDialog >();
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