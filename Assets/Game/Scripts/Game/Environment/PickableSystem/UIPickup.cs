using StarSmithGames.Go;
using System;

namespace Game.Environment.PickableSystem
{
	public sealed class UIPickup : ViewPopupBase
	{
		public event Action OnButtonClicked;
		
		public void OnButtonClick()
		{
			OnButtonClicked?.Invoke();
		}
	}
}