using Game.Scripts.Extensions;
using UnityEngine;

namespace Game.UI
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