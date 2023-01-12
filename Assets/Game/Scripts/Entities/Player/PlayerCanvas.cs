using Game.Systems.CameraSystem;

using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public class PlayerCanvas : MonoBehaviour
	{
		[field: SerializeField] public UILockpickBar LockpickBar { get; private set; }

		private CameraSystem cameraSystem;

		[Inject]
		private void Construct(CameraSystem cameraSystem)
		{
			this.cameraSystem = cameraSystem;
		}

		private void Update()
		{
			transform.rotation = cameraSystem.Rotation;
		}
	}
}