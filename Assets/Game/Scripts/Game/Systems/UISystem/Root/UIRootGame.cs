using UnityEngine;

namespace Game.UI
{
	public class UIRootGame : UIRoot
	{
		public UIGameCanvas gameCanvas;

		public override Transform GetDialogsRoot() => gameCanvas.dialogsRoot;
	}
}