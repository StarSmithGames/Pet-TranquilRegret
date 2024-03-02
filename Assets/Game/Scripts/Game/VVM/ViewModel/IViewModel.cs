using System;
using Zenject;

namespace Game.VVM
{
	public interface IViewModel : IInitializable, IDisposable
	{
		event Action< IViewModel > OnDisposed;
		
		void EnableView( bool trigger );
		void ShowView( Action callback = null );
		void HideView( Action callback = null );
		void HideViewAndDispose( Action callback = null );
	}
}