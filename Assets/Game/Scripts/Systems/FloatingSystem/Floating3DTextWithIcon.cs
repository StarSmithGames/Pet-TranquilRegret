using DG.Tweening;
using UnityEngine;

using Zenject;

namespace Game.Systems.FloatingSystem
{
	public class Floating3DTextWithIcon : Floating3DText
	{
		[field: SerializeField] public SpriteRenderer Icon { get; private set; }

		public override void SetFade(float endValue)
		{
			Icon.DOFade(endValue, 0.01f);
			Text.DOFade(endValue, 0.01f);
		}

		public override Tween Fade(float endValue, float duration)
		{
			Sequence sequence = DOTween.Sequence();

			sequence
				.Append(Icon.DOFade(endValue, duration))
				.Join(Text.DOFade(endValue, duration));

			return sequence;
		}

		public new class Factory : PlaceholderFactory<Floating3DTextWithIcon> { }
	}
}