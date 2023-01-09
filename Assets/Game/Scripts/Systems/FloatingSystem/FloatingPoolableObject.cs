using DG.Tweening;

namespace Game.Systems.FloatingSystem
{
	public abstract class FloatingPoolableObject : PoolableObject
	{
		public abstract void SetFade(float endValue);

		public abstract Tween Fade(float endValue, float duration);
	}
}