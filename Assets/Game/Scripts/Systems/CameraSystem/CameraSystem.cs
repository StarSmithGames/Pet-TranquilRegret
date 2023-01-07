using Cinemachine;

using Game.Managers.CharacterManager;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Zenject;

namespace Game.Systems.CameraSystem
{
	public class CameraSystem : MonoBehaviour
	{
		[SerializeField] private CinemachineBrain brain;
		[SerializeField] private List<CinemachineVirtualCamera> camers = new List<CinemachineVirtualCamera>();
		[SerializeField] private Transform target;

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

			SetTarget(characterManager.CurrentPlayer?.transform);
		}

		private void OnDestroy()
		{
			signalBus?.Unsubscribe<SignalPlayerChanged>(OnPlayerChanged);
		}


		public void SetTarget(Transform target)
		{
			this.target = target;

			camers.ForEach((x) =>
			{
				x.Follow = target;
				x.LookAt = target;
			});
		}

		private IEnumerator WaitWhileCamerasBlendes()
		{
			yield return null;
			yield return new WaitWhile(() => !brain.ActiveBlend?.IsComplete ?? false);

			CurrentTransposer = (brain.ActiveVirtualCamera as CinemachineVirtualCamera).GetCinemachineComponent<CinemachineFramingTransposer>();
		}

		private void OnPlayerChanged(SignalPlayerChanged signal)
		{
			SetTarget(signal.player?.transform);
		}


		[Button(DirtyOnClick = true)]
		private void Refresh()
		{
			SetTarget(target);
		}
	}
}