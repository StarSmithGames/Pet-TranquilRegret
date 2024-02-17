using StarSmithGames.Go;
using System;

namespace Game.Systems.UISystem
{
	public interface IUIView : IView
	{
		event Action< IUIView > OnDisposed;

		void Dispose();
	}
}