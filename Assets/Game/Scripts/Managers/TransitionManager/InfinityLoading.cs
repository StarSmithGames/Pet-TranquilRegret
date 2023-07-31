using StarSmithGames.Go;

namespace Game.Managers.TransitionManager
{
	public class InfinityLoading : ViewBase
	{
		public TMPro.TextMeshProUGUI progress;

		private void Awake()
		{
			Enable(false);
		}
	}
}