using Game.Managers.AsyncManager;

using Sirenix.OdinInspector;
using UnityEngine;

using Zenject;

namespace Game.VFX
{
	public class ParticalVFX : MonoBehaviour
	{
		[SerializeField] private bool isEnableOnStart = false;
		[SerializeField] private bool isEmit = false;
		[ShowIf("isEmit")]
		[SerializeField] private int emitCount = 50;
		[SerializeField] private ParticleSystem particleSystem;

		private void Start()
		{
			Enable(isEnableOnStart);
		}

		public void Enable(bool trigger)
		{
			if (trigger)
			{
				if (particleSystem.isPlaying) return;

				if (isEmit)
				{
					particleSystem.Emit(emitCount);
				}
				else
				{
					particleSystem.Play();
				}
			}
			else
			{
				if (particleSystem.isStopped) return;

				particleSystem.Stop();
			}
		}
	}

	public abstract class PoolableParticalVFX : PoolableObject
	{
		public bool IsPlaying { get; private set; }

		[SerializeField] private bool isEmit = false;
		[SerializeField] private int emitCount = 50;
		[SerializeField] private ParticleSystem particleSystem;

		private float duration;
		private float t = 0;

		private void Start()
		{
			duration = particleSystem.main.duration;
		}

		private void Update()
		{
			if (!IsPlaying) return;

			t += Time.deltaTime;

			if(t > duration)
			{
				DespawnIt();
			}
		}

		public void Play()
		{
			t = 0;

			if (isEmit)
			{
				particleSystem.Emit(emitCount);
			}
			else
			{
				particleSystem.Play();
			}

			IsPlaying = true;
		}

		public override void OnDespawned()
		{
			particleSystem.Stop();

			IsPlaying = false;

			base.OnDespawned();
		}
	}
}