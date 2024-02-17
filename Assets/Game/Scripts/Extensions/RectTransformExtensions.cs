using UnityEngine;

namespace Game.Scripts.Extensions
{
	public static class RectTransformExtensions
	{
		public static Vector3[] GetImageCorners( this RectTransform rt )
		{
			var imageCorners = new Vector3[4];
			rt.GetWorldCorners( imageCorners );
			return imageCorners;
		}
        
		public static float GetImageHeight( this RectTransform rt )
		{
			var imageCorners = new Vector3[4];
			rt.GetWorldCorners( imageCorners );
			float imageHeight = imageCorners[ 1 ].y - imageCorners[ 0 ].y;
			return imageHeight;
		}
        
		public static float GetImageWidth( this RectTransform rt )
		{
			var imageCorners = new Vector3[4];
			rt.GetWorldCorners( imageCorners );
			float imageWidth = imageCorners[ 3 ].x - imageCorners[ 0 ].x;
			return imageWidth;
		}
        
		public static void SetLeft(this RectTransform rt, float left)
		{
			rt.offsetMin = new Vector2(left, rt.offsetMin.y);
		}

		public static void SetRight(this RectTransform rt, float right)
		{
			rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
		}

		public static void SetTop(this RectTransform rt, float top)
		{
			rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
		}

		public static void SetBottom(this RectTransform rt, float bottom)
		{
			rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
		}
	}
}