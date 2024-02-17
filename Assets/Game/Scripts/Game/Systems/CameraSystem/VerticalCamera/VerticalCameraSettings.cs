using UnityEngine;

namespace Game.Systems.CameraSystem
{
	[ System.Serializable ]
	public sealed class VerticalCameraSettings
	{
		public float MouseSensitivity = 0.1f;
		public float DragDamping = 0.0f;
		[Space]
		public Vector3 StartPoint = new( 0, 95, 0);
		public Vector3 TopOffset = Vector3.zero;
	}
}