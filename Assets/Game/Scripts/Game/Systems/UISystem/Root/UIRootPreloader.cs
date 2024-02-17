using UnityEngine;

namespace Game.UI
{
	public sealed class UIRootPreloader : UIRoot
	{
		public Transform DialogRoot;
		
		public override Transform GetDialogsRoot() => DialogRoot;
	}
}