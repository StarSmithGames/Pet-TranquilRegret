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

		public DecalProjector projector;
		public List<DecalProjector> projectors = new List<DecalProjector>();
		[OnValueChanged("UpdateSize")]
		public float depth = 10;
		[OnValueChanged("UpdateSize")]
		public Vector2 size = Vector2.one;

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
			projector.enabled = IsEnabled;
		}

		public Tween DoIdle()
		{
			idleSequence = DOTween.Sequence();

			idleSequence
				.AppendCallback(() => projector.DORewind())
				.Append(DOTween.Punch(() => originalSize, (x) => SetScale(new Vector3(x.x, x.y, depth)), new Vector3(0.25f, 0.25f, 0), 0.25f))
				.SetLoops(-1);

			return idleSequence;
		}

		#region Scale
		public void SetScale(Vector3 size)
		{
			for (int i = 0; i < projectors.Count; i++)
			{
				projectors[i].size = size;
			}
		}

		public void SetSizeFactor(float sizeFactor)
		{
			this.sizeFactor = sizeFactor;
			for (int i = 0; i < projectors.Count; i++)
			{
				projectors[i].size = new Vector3(originalSize.x * sizeFactor, originalSize.y * sizeFactor, depth);
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
		#endregion

		#region Fade
		public DecalVFX SetFade(float value)
		{
			for (int i = 0; i < projectors.Count; i++)
			{
				projectors[i].fadeFactor = value;
			}

			return this;
		}

		public void FadeTo(float end, float duration = 0.25f, UnityAction callback = null)
		{
			Enable(true);

			if (end == 0)
			{
				DOTween
					.To(() => projector.fadeFactor, x => SetFade(x), 0f, duration)
					.OnComplete(() =>
					{
						Enable(false);
						callback?.Invoke();
					});
			}
			else
			{
				DOTween
					.To(() => projector.fadeFactor, x => SetFade(x), end, duration)
					.OnComplete(() =>
					{
						callback?.Invoke();
					});
			}
		}
		#endregion

		#region Color
		public void SetColor(Color color)
		{
			if(sharedMaterial == null)
			{
				sharedMaterial = new Material(projector.material);
				projector.material = sharedMaterial;
			}

			sharedMaterial.color = color;
		}
		#endregion

		public void DoKill()
		{
			idleSequence.Kill(true);
			idleSequence = null;

			projector.DOKill(true);
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