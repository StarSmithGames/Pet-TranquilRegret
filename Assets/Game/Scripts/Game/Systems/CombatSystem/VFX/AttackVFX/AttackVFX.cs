using Game.VFX;
using UnityEngine;

namespace Game.Systems.CombatSystem
{
	public abstract class AttackVFX : DespawnablePoolableObject
	{
		public ParticleSystem ParticleSystem;
		
		public virtual void Play()
		{
			ParticleSystem.Stop();
			ParticleSystem.Play();
		}
	}
}