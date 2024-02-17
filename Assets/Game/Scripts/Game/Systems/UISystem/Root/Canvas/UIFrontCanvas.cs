using Game.Scripts.Extensions;
using UnityEngine;

namespace Game.Systems.UISystem
{
	public sealed class UIFrontCanvas : UICanvas
	{
		public RectTransform Bottom;

		public float GetWorldHeight()
		{
			return Bottom.GetImageHeight();
		}
	}
}