using Game.Systems.UISystem;
using StarSmithGames.Go;
using System;

namespace Game.UI
{
	public class UIViewDialog : ViewBase, IUIView
	{
		public event Action< IUIView > OnDisposed;

		public void HideAndDispose( Action callback = null )
		{
			Hide( () =>
			{
				callback?.Invoke();
				Dispose();
			} );
		}
		
		public void Dispose()
		{
			ViewDestroyer.Destroy( this );
			OnDisposed?.Invoke( this );
		}
	}
}