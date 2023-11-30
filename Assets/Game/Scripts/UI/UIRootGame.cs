using Game.Systems.StorageSystem;

using UnityEngine;

using Zenject;

namespace Game.UI
{
	public class UIRootGame : MonoBehaviour
	{
		public UIGameCanvas gameCanvas;

		[Inject] private GameData gameData;

		private void Awake()
		{
			gameData.IntermediateData.RootGame = this;
		}

		private void OnDestroy()
		{
			gameData.IntermediateData.RootGame = null;
		}
	}
}