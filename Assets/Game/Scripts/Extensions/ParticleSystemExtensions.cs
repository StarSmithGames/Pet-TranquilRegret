using UnityEngine;

namespace Game.Scripts.Extensions
{
	public static class ParticleSystemExtensions
	{
		public static void PlayNow( this ParticleSystem particleSystem )
		{
			particleSystem.Stop( true );
			particleSystem.Play( true );
		}

	}
}