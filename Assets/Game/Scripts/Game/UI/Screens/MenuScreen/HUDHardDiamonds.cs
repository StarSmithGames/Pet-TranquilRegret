using Game.Systems.StorageSystem;
using UnityEngine;

using Zenject;

namespace Game.UI
{
	public class HUDHardDiamonds : MonoBehaviour
	{
		public TMPro.TextMeshProUGUI text;

		[Inject] private StorageSystem storageSystem;

		private void Awake()
		{
			storageSystem.GameFastData.HardDiamonds.onChanged += OnDiamondsChanged;
			OnDiamondsChanged();
		}

		private void OnDestroy()
		{
			storageSystem.GameFastData.HardDiamonds.onChanged -= OnDiamondsChanged;
		}

		private void OnDiamondsChanged()
		{
			text.text = storageSystem.GameFastData.HardDiamonds.Value.ToString();
		}
	}
}