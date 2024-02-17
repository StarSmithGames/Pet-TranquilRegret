using StarSmithGames.Go;
using UnityEngine;

namespace Game.Systems.UISystem
{
	public static class ViewDestroyer
	{
		public static void Destroy( ViewBase view )
		{
			GameObject.Destroy( view.gameObject );
		}
		
		public static void DestroyImmediate( ViewBase view )
		{
			GameObject.DestroyImmediate( view.gameObject );
		}
	}
}