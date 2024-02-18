using Cysharp.Threading.Tasks;
using System.Threading;

using UnityEngine;

namespace Game.Systems.LevelSystem
{
    public sealed class LevelTimer : Timer
	{
		public void StartTimer()
		{
			_timerToken?.Dispose();
			_timerToken = new();
			Timer(_timerToken.Token).Forget();
		}

		private async UniTask Timer( CancellationToken cancellation )
		{
			while (true)
			{
				while (_isPaused)
				{
					await UniTask.Yield(cancellation);
				}

				_remainingTime += Time.deltaTime;

				await UniTask.Yield(cancellation);
			}
		}
	}
}