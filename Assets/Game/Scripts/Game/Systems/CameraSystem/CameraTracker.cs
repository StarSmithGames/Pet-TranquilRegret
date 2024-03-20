using UnityEngine;

using Zenject;

namespace Game.Systems.CameraSystem
{
	public abstract class CameraTracker : MonoBehaviour
	{
		protected CameraSystem _cameraSystem;

		[Inject]
		private void Construct(CameraSystem cameraSystem)
		{
			_cameraSystem = cameraSystem;
		}

		protected virtual void Update()
		{
			transform.rotation = _cameraSystem.Rotation;
		}
	}
}