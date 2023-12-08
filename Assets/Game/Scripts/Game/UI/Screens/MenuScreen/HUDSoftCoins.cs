using Game.Systems.StorageSystem;

using UnityEngine;

using Zenject;

namespace Game.UI
{
	public class HUDSoftCoins : MonoBehaviour
	{
		public TMPro.TextMeshProUGUI text;

		[Inject] private StorageSystem storageSystem;

		private void Awake()
		{
			storageSystem.GameFastData.SoftCoins.onChanged += OnCoinsChanged;
			OnCoinsChanged();
		}

		private void OnDestroy()
		{
			storageSystem.GameFastData.SoftCoins.onChanged -= OnCoinsChanged;
		}

		private void OnCoinsChanged()
		{
			text.text = storageSystem.GameFastData.SoftCoins.Value.ToString();
		}
	}
}