using Cinemachine;

using Game.Character;
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

		public Camera Camera => brain.OutputCamera;

		public CinemachineBrain brain;
		public List<CinemachineVirtualCamera> camers = new List<CinemachineVirtualCamera>();
#if UNITY_EDITOR
		public Transform lookAtPivotEditor;
		public Transform folowPivotEditor;
#endif

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

		private void Start()
		{
			outputCamera = brain.OutputCamera.transform;
		}

		public CameraSystem SetTarget(AbstractCharacter character)
		{
			return SetTarget(character.facade.cameraFollowPivot, character.facade.cameraLookAtPivot);
		}

		public CameraSystem SetTarget(Transform target)
		{
			camers.ForEach((x) =>
			{
				x.Follow = target;
				x.LookAt = target;
			});

			return this;
		}

		public CameraSystem SetTarget(Transform follow, Transform lookAt)
		{
			camers.ForEach((x) =>
			{
				x.Follow = follow;
				x.LookAt = lookAt;
			});

			return this;
		}

		public void SetTracketOffsetDirection(Vector3 tracketObjectOffset)
		{
			CurrentTransposer.m_TrackedObjectOffset = tracketObjectOffset;
		}

		private IEnumerator WaitWhileCamerasBlendes()
		{
			yield return null;
			yield return new WaitWhile(() => !brain.ActiveBlend?.IsComplete ?? false);

			CurrentTransposer = (brain.ActiveVirtualCamera as CinemachineVirtualCamera).GetCinemachineComponent<CinemachineFramingTransposer>();
		}
	}
}