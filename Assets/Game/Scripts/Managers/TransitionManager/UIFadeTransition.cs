using Game.UI;

namespace Game.Managers.TransitionManager
{
    public class UIFadeTransition : WindowBase, ITransitable
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