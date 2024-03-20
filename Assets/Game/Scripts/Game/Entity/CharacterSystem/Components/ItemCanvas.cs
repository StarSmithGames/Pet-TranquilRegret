using Game.Environment.PickableSystem;
using Game.Systems.CameraSystem;
using UnityEngine;

namespace Game.Entity.CharacterSystem
{
	public sealed class ItemCanvas : CameraTracker
	{
		public Canvas Canvas;
		public UIPickup Pickup;

		private void Start()
		{
			Canvas.worldCamera = _cameraSystem.Camera;
		}
	}
}