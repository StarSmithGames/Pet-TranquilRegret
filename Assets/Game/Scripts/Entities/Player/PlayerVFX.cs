using UnityEngine;

namespace Game.Entities
{
	public class PlayerVFX : MonoBehaviour
	{
		[SerializeField] private ParticleSystem dustEffect;

		public void EnableDust(bool trigger)
		{
			if (trigger)
			{
				if (dustEffect.isPlaying) return;

				dustEffect.Play();
			}
			else
			{
				if (dustEffect.isStopped) return;

				dustEffect.Stop();
			}
		}
	}
}