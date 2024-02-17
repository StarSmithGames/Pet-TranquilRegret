using UnityEngine;

namespace Game.Systems.CameraSystem
{
	public static class CameraUtilits
	{
		public static readonly Vector2 targetAspectRatio = new( 9f, 16f );

		public static float GetAspectTarget() => targetAspectRatio.x / targetAspectRatio.y;
	}
}