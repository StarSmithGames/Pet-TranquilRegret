using Game.Environment.EntitySystem;
using Game.Scripts.Extensions;
using UnityEngine;

namespace Game.Environment.DestructableSystem
{
	public abstract class DestructableMonoObject : MonoObject
	{
		public ParticleSystem DestructEffect;

		public override void Destruct()
		{
			if ( DestructEffect )
			{
				DestructEffect.transform.SetParent( null );
				DestructEffect.PlayNow();
			}
			
			base.Destruct();
		}
	}
}