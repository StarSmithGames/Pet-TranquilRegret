using Game.Scripts.Extensions;
using UnityEngine;

namespace Game.UI
{
	public sealed class UINavigationCanvas : UICanvas
	{
		public RectTransform TabsContent;
		
		public float GetWorldHeight()
		{
			return TabsContent.GetImageHeight();
		}
	}
}