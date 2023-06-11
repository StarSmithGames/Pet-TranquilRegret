using Game.UI;

using StarSmithGames.Go;

namespace Game.Managers.TransitionManager
{
    public class UIFadeTransition : ViewBase, ITransitable
    {
		private void Start()
		{
			Enable(false);
		}

		public void Terminate()
		{
		}
    }
}