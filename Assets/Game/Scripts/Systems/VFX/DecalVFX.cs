using DG.Tweening;

using Sirenix.OdinInspector;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace Game.VFX
{
	public class DecalVFX : MonoBehaviour
	{
		public bool IsEnabled { get; private set; } = true;

		[SerializeField] private DecalProjector masterProjector;
		[SerializeField] private List<DecalProjector> projectors = new List<DecalProjector>();
		[OnValueChanged("UpdateSize")]
		[SerializeField] private float depth = 10;
		[OnValueChanged("UpdateSize")]
		[SerializeField] private Vector2 size = Vector2.one;

		private float sizeFactor = 1f;
		private Vector2 originalSize;
		private Material sharedMaterial;
		private Sequence idleSequence;

		private void Start()
		{
			originalSize = size;
		}

		public void Enable(bool trigger)
		{
			IsEnabled = trigger;
			masterProjector.enabled = IsEnabled;
		}

		public void StartIdleAnimation()
		{
			idleSequence = DOTween.Sequence();

			idleSequence
				.AppendCallback(() => masterProjector.DORewind())
				.Append(DOTween.Punch(() => originalSize, (x) => SetScale(new Vector3(x.x, x.y, depth)), new Vector3(0.25f, 0.25f, 0), 0.25f))
				.SetLoops(-1);
		}

		public void SetSizeFactor(float sizeFactor)
		{
			this.sizeFactor = sizeFactor;
			masterProjector.size = new Vector3(originalSize.x * sizeFactor, originalSize.y * sizeFactor, 10);
		}

		public void SetScale(Vector3 size)
		{
			for (int i = 0; i < projectors.Count; i++)
			{
				projectors[i].size = size;
			}
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
			masterProjector.fadeFactor = value;
			Enable(masterProjector.fadeFactor != 0);

			return this;
		}

		public void FadeTo(float end, float duration = 0.25f, UnityAction callback = null)
		{
			Enable(true);

			if (end == 0)
			{
				DOTween
					.To(() => masterProjector.fadeFactor, x => masterProjector.fadeFactor = x, 0f, duration)
					.OnComplete(() =>
					{
						Enable(false);
						callback?.Invoke();
					});
			}
			else
			{
				DOTween
					.To(() => masterProjector.fadeFactor, x => masterProjector.fadeFactor = x, end, duration)
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
				sharedMaterial = new Material(masterProjector.material);
				masterProjector.material = sharedMaterial;
			}

			sharedMaterial.color = color;
		}


		public void Kill()
		{
			idleSequence.Kill(true);
			idleSequence = null;

			masterProjector.DOKill(true);
			transform.DOKill(true);
		}

		private void UpdateSize()
		{
			projectors.ForEach((x) =>
			{
				x.size = new Vector3(size.x, size.y, depth);
			});
		}

		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			projectors = GetComponentsInChildren<DecalProjector>().ToList();
			UpdateSize();
		}
	}
}