
using StarSmithGames.Go;

using UnityEngine.UI;

namespace Game.Managers.TransitionManager
{
	public class InfinityLoading : ViewBase
	{
		public Image fill;

		private void Awake()
		{
			Enable(false);

			fill.fillAmount = 0;
		}

		public void SetProgress(float value)
		{
			fill.fillAmount = value;
		}
	}
}