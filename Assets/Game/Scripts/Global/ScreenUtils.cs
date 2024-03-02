using UnityEngine;

namespace Game
{
	public class ScreenUtils
	{
		public static readonly Vector2 targetAspectRatio = new( 9f, 16f );

		public static float GetAspectCurrent() => Camera.main.aspect;

		public static Vector2 ScreenSizeInUnits( Camera camera = null )
		{
			if ( camera == null ) camera = Camera.main;

			float height = camera.orthographicSize * 2;
			float width = camera.aspect * height;

			return new Vector2( width, height );
		}

		public static Vector2 ScreenSizeInUnitsForCameraSize( float cameraSize, Camera camera = null )
		{
			if ( camera == null ) camera = Camera.main;

			float height = cameraSize * 2;
			float width = camera.aspect * height;

			return new Vector2( width, height );
		}

		public static float GetRatioDiffrence()
		{
			float ratio = Mathf.Abs( GetAspectFullHDPortrait() - GetAspectCurrent() );
			return ratio > 0.001f ? ratio : 0;
		}

		public static Vector2 GetAspectSizeNormal()
		{
			var currentScreenResolution = new Vector2( Screen.width, Screen.height );
			Vector2 normalizedAspectRatio = targetAspectRatio / currentScreenResolution;

			return normalizedAspectRatio / Mathf.Max( normalizedAspectRatio.x, normalizedAspectRatio.y );
		}

		public static bool IsAspectPreferredHeight()
		{
			Vector2 aspectSizeDiffrence = GetAspectSizeNormal();

			if ( aspectSizeDiffrence == Vector2.one || aspectSizeDiffrence.y != 1f ) return true;

			return false;
		}

		public static float GetAspectFullHDPortrait() => targetAspectRatio.x / targetAspectRatio.y;

		public static float GetScaleWidthRatio() => GetAspectCurrent() / GetAspectFullHDPortrait();

		public static float GetScaleHeightRatio() => GetAspectFullHDPortrait() / GetAspectCurrent();
	}
}