using UnityEngine;

using Zenject;

namespace Game.Systems.CameraSystem
{
	public abstract class CameraTracker : MonoBehaviour
	{
		private CameraSystem cameraSystem;

		[Inject]
		private void Construct(CameraSystem cameraSystem)
		{
			this.cameraSystem = cameraSystem;
		}

		protected virtual void Update()
		{
			transform.rotation = cameraSystem.Rotation;
		}
	}
}