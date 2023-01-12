using Cinemachine;

using Game.Managers.CharacterManager;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unity.VisualScripting;

using UnityEngine;

using Zenject;

namespace Game.Systems.CameraSystem
{
	public class CameraSystem : MonoBehaviour
	{
		public Vector3 Forward => outputCamera.forward;
		public Vector3 Right => outputCamera.right;

		public float EulerAngleY => outputCamera.eulerAngles.y;
		public Quaternion Rotation => outputCamera.rotation;

		[SerializeField] private CinemachineBrain brain;
		[SerializeField] private List<CinemachineVirtualCamera> camers = new List<CinemachineVirtualCamera>();
		[SerializeField] private float cameraDistance = 15f;

		private Transform outputCamera;
		private CinemachineFramingTransposer CurrentTransposer
		{
			set
			{
				currentTransposer = value;
			}
			get
			{
				if (currentTransposer == null)
				{
					if (brain.ActiveVirtualCamera == null)
					{
						currentTransposer = camers.FirstOrDefault()?.GetCinemachineComponent<CinemachineFramingTransposer>();
					}
					else
					{
						currentTransposer = (brain.ActiveVirtualCamera as CinemachineVirtualCamera).GetCinemachineComponent<CinemachineFramingTransposer>();
					}
				}
				return currentTransposer;
			}
		}
		private CinemachineFramingTransposer currentTransposer;

		private SignalBus signalBus;
		private CharacterManager characterManager;

		[Inject]
		public void Construct(SignalBus signalBus, CharacterManager characterManager)
		{
			this.signalBus = signalBus;
			this.characterManager = characterManager;
		}

		private void Start()
		{
			signalBus?.Subscribe<SignalPlayerChanged>(OnPlayerChanged);

			outputCamera = brain.OutputCamera.transform;

			SetTarget(characterManager.CurrentPlayer.CameraFollowPivot, characterManager.CurrentPlayer.CameraLookAtPivot);
		}

		private void OnDestroy()
		{
			signalBus?.Unsubscribe<SignalPlayerChanged>(OnPlayerChanged);
		}


		public void SetTarget(Transform target)
		{
			camers.ForEach((x) =>
			{
				x.Follow = target;
				x.LookAt = target;
			});
		}

		public void SetTarget(Transform follow, Transform lookAt)
		{
			camers.ForEach((x) =>
			{
				x.Follow = follow;
				x.LookAt = lookAt;
			});
		}

		public void SetTracketOffsetDirection(Vector3 tracketObjectOffset)
		{
			CurrentTransposer.m_TrackedObjectOffset = tracketObjectOffset * cameraDistance;
		}

		private IEnumerator WaitWhileCamerasBlendes()
		{
			yield return null;
			yield return new WaitWhile(() => !brain.ActiveBlend?.IsComplete ?? false);

			CurrentTransposer = (brain.ActiveVirtualCamera as CinemachineVirtualCamera).GetCinemachineComponent<CinemachineFramingTransposer>();
		}

		private void OnPlayerChanged(SignalPlayerChanged signal)
		{
			SetTarget(signal.player?.CameraFollowPivot, signal.player?.CameraLookAtPivot);
		}
	}
}