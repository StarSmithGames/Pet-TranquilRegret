using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Systems.LevelSystem
{
	public sealed class EstimatedTimer : Timer
	{
		public event Action onTimeOut;
		
		public void StartEstimatedTimer( float estimatedTime )
		{
			_remainingTime = estimatedTime;

			_timerToken?.Dispose();
			_timerToken = new();
			Timer(_timerToken.Token).Forget();
		}
		
		private async UniTask Timer( CancellationToken cancellation )
		{
			while (_remainingTime > 0)
			{
				while (_isPaused)
				{
					await UniTask.Yield(cancellation);
				}

				_remainingTime -= Time.deltaTime;
				TimeSpan span = TimeSpan.FromSeconds(_remainingTime);
				if (span.TotalSeconds <= 0)
				{
					_remainingTime = 0;
					onTimeOut?.Invoke();
				}

				await UniTask.Yield(cancellation);
			}

			Stop();
		}
	}
}