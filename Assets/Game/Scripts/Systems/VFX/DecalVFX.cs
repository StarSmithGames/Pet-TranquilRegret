using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace Game.VFX
{
	public class DecalVFX : MonoBehaviour
	{
		public bool IsEnabled { get; private set; } = true;

		[SerializeField] protected DecalProjector projector;

		private float sizeFactor = 1f;
		private Vector2 originalSize;
		private Material sharedMaterial;
		private Sequence idleSequence;

		private void Start()
		{
			originalSize = projector.size;
		}

		public void Enable(bool trigger)
		{
			IsEnabled = trigger;
			projector.enabled = IsEnabled;
		}

		public void StartIdleAnimation()
		{
			idleSequence = DOTween.Sequence();

			idleSequence
				.AppendCallback(() => projector.DORewind())
				.Append(DOTween.Punch(() => originalSize, (x) => projector.size = new Vector3(x.x, x.y, 10), new Vector3(0.25f, 0.25f, 0), 0.25f))
				.SetLoops(-1);
		}

		public void SetSizeFactor(float sizeFactor)
		{
			this.sizeFactor = sizeFactor;
			projector.size = new Vector3(originalSize.x * sizeFactor, originalSize.y * sizeFactor, 10);
		}

		public void ScaleTo(float end, float duration = 0.25f, UnityAction callback = null)
		{
			if (end == 0)
			{
				DOTween
					.To(() => sizeFactor, x => SetSizeFactor(x), 0f, duration)
					.OnComplete(() =>
					{
						Enable(false);
						callback?.Invoke();
					});
			}
			else
			{
				DOTween
					.To(() => sizeFactor, x => SetSizeFactor(x), end, duration)
					.OnComplete(() =>
					{
						callback?.Invoke();
					});
			}
		}

		public DecalVFX SetFade(float value)
		{
			projector.fadeFactor = value;
			Enable(projector.fadeFactor != 0);

			return this;
		}

		public void FadeTo(float end, float duration = 0.25f, UnityAction callback = null)
		{
			Enable(true);

			if (end == 0)
			{
				DOTween
					.To(() => projector.fadeFactor, x => projector.fadeFactor = x, 0f, duration)
					.OnComplete(() =>
					{
						Enable(false);
						callback?.Invoke();
					});
			}
			else
			{
				DOTween
					.To(() => projector.fadeFactor, x => projector.fadeFactor = x, end, duration)
					.OnComplete(() =>
					{
						callback?.Invoke();
					});
			}
		}

		public void SetColor(Color color)
		{
			if(sharedMaterial == null)
			{
				sharedMaterial = new Material(projector.material);
				projector.material = sharedMaterial;
			}

			sharedMaterial.color = color;
		}


		public void Kill()
		{
			idleSequence.Kill(true);
			idleSequence = null;

			projector.DOKill(true);
			transform.DOKill(true);
		}
	}
}