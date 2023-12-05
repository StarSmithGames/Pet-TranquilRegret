using Game.Systems.StorageSystem;

using UnityEngine;

using Zenject;

namespace Game.UI
{
	public class UIRootGame : UIRoot
	{
		public UIGameCanvas gameCanvas;

		[Inject] private StorageSystem gameData;

		private void Awake()
		{
			gameData.IntermediateData.RootGame = this;
		}

		private void OnDestroy()
		{
			gameData.IntermediateData.RootGame = null;
		}

		public override Transform GetDialogsRoot()
		{
			return gameCanvas.dialogsRoot;
		}
	}
}