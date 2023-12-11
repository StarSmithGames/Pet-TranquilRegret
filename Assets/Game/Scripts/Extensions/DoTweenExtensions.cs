using DG.Tweening;

using UnityEngine;

namespace Game.Extensions
{
	public static class DoTweenExtensions
	{
		public static Tween DOPunchScale(this Transform transform, PunchSettings settings)
		{
			return transform.DOPunchScale(settings.GetPunch(), settings.duration, settings.vibrato, settings.elasticity);
		}
	}
}