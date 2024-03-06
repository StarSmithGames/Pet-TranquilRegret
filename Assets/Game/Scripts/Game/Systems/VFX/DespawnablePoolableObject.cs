using Cysharp.Threading.Tasks;
using StarSmithGames.IoC;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Game.VFX
{
	public abstract class DespawnablePoolableObject : PoolableObject
	{
		public float WaitTime = 5f;

		private CancellationTokenSource _cancellationTokenSource;

		public override void OnSpawned( IMemoryPool pool )
		{
			base.OnSpawned( pool );
			
			if ( _cancellationTokenSource != null )
			{
				Debug.LogError( "[DespawnablePoolableObject] Collision" );
				
				_cancellationTokenSource.Dispose();
				_cancellationTokenSource = null;
			}
			
			_cancellationTokenSource = new();
			Waiter( _cancellationTokenSource.Token ).Forget();
		}

		public override void OnDespawned()
		{
			base.OnDespawned();
			
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = null;
		}

		private async UniTask Waiter( CancellationToken token )
		{
			await UniTask.WaitForSeconds( WaitTime, cancellationToken: token );
			
			DespawnIt();
		}
	}
}