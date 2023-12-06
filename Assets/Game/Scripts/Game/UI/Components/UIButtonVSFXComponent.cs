using Game.Services;

using StarSmithGames.Go;

using UnityEngine.EventSystems;

using Zenject;

namespace Game.UI
{
	public class UIButtonVSFXComponent : UIButtonComponent
	{
		[Inject] protected VSFXService vsfxService;

		public override void OnPointerClick(PointerEventData eventData)
		{
			vsfxService.PlayUIButton();
		}
	}
}