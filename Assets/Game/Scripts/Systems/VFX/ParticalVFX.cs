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
		public bool IsPlaying { get; protected set; }

		[SerializeField] protected ParticleSystem particleSystem;

		[SerializeField] protected bool isCustomDuration = false;
		[Min(0.1f)]
		[ShowIf("isCustomDuration")]
		[SerializeField] protected float customDuration = 0.1f;
		[SerializeField] protected int emitCount = 50;

		protected float duration;
		protected float t = 0;

		private void Start()
		{
			duration = isCustomDuration ? customDuration : particleSystem.main.duration;
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
	
		[Button]
		public virtual void Play()
		{
			t = 0;

			particleSystem.Emit(emitCount);

			IsPlaying = true;
		}

		public virtual void Play(ParticleSystem.EmitParams emit)
		{
			t = 0;

			particleSystem.Emit(emit, emitCount);

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