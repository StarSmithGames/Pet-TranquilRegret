using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Systems.CombatSystem;
using UnityEngine;

namespace Game.Environment.EntitySystem
{
	public sealed class MonoMailboxObject : MonoObject
	{
		private bool _isInitialized;
		private Vector3 _cachedForward;
		
		public override void TakeDamage( Damage damage )
		{
			if ( !_isInitialized )
			{
				_isInitialized = true;
				_cachedForward = transform.forward;
			}

			Animation( damage.AttackDirection * 30f ).Forget();
			// Destruct();
		}

		public override void Destruct()
		{
			//puff
			base.Destruct();
		}

		private async UniTask Animation( Vector3 strength )
		{
			await transform.DOShakeRotation( Random.Range( 0.33f, 1.66f ), strength, 5 );
			await transform.DORotate(  Quaternion.LookRotation( _cachedForward ).eulerAngles, Random.Range( 0.33f, 0.99f ) );
		}
	}
}