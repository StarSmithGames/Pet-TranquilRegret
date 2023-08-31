using Game.Managers.CharacterManager;
using Game.Systems.CameraSystem;

using StarSmithGames.Core;

using UnityEngine;

using Zenject;

namespace Game.VFX.Markers
{
    public sealed class Marker : MonoBehaviour, IEnableable
    {
		public bool IsEnable { get; private set; } = false;

		private IPointer pointer;

		//private UIPointer.Factory factoryPointerUI;
		private CharacterManager characterManager;
		private CameraSystem cameraSystem;

		[Inject]
		private void Construct(/*UIPointer.Factory factoryPointerUI, */CharacterManager characterManager, CameraSystem cameraSystem)
		{
			//this.factoryPointerUI = factoryPointerUI;
			this.characterManager = characterManager;
			this.cameraSystem = cameraSystem;
		}

		private void Update()
		{
			if (IsEnable)
			{
				Vector3 playerPosition = characterManager.CurrentPlayer.transform.position;
				Vector3 direction = transform.position - playerPosition;

				Ray ray = new Ray(playerPosition, direction);

				//[0]-left [1]-right [2]-up [3]-down
				var panels = GeometryUtility.CalculateFrustumPlanes(cameraSystem.Camera);
				int planeIndex = 0;

				float minDistance = Mathf.Infinity;

				for (int i = 0; i < panels.Length; i++)
				{
					if (panels[i].Raycast(ray, out float distance))
					{
						if (distance < minDistance)
						{
							minDistance = distance;
							planeIndex = i;
						}
					}
				}

				if (direction.magnitude > minDistance)//show
				{
					if (pointer == null)
					{
						CreatePointer();
					}

					pointer.Transform.position = cameraSystem.Camera.WorldToScreenPoint(ray.GetPoint(minDistance));
					pointer.Transform.rotation = GetIconRotation(planeIndex);
				}
				else//hide
				{
					DisposePointer();
				}
			}
		}

		public void Enable(bool trigger)
		{
			IsEnable = trigger;

			if (!IsEnable)
			{
				DisposePointer();
			}
		}

		private Quaternion GetIconRotation(int planeIndex)
		{
			if (planeIndex == 0)
			{
				return Quaternion.Euler(0, 0, -90f);
			}
			else if (planeIndex == 1)
			{
				return Quaternion.Euler(0, 0, 90f);
			}
			else if (planeIndex == 2)
			{
				return Quaternion.Euler(0, 0, 0);
			}

			return Quaternion.Euler(0, 0, 180f);
		}

		private void CreatePointer()
		{
			//pointer = factoryPointerUI.Create();
		}

		private void DisposePointer()
		{
			if (pointer != null)
			{
				pointer.DespawnIt();
				pointer = null;
			}
		}

		
	}
}