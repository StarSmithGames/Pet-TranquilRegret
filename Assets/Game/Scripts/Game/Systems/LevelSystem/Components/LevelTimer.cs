using Company.Module.Utils;

using Cysharp.Threading.Tasks;

using System;
using System.Threading;

using UnityEngine;

namespace Game.Systems.LevelSystem
{
    public class LevelTimer
    {
		public event Action onTimeOut;

		public float Ticks => ticks;
		private float ticks;
		private bool isPaused;

		private CancellationTokenSource timerToken;

		public string GetTime()
		{
			return TimeUtils.GetTimerFormatHMS(TimeSpan.FromSeconds(ticks));
		}

		public int GetMinutes()
		{
			return TimeSpan.FromSeconds(ticks).Minutes;
		}

		public int GetSeconds()
		{
			return TimeSpan.FromSeconds(ticks).Seconds;
		}

		public int GetMilliseconds()
		{
			return TimeSpan.FromSeconds(ticks).Milliseconds / 10;
		}

		public void StartEstimatedTimer(float estimatedTime)
		{
			ticks = estimatedTime;

			timerToken?.Dispose();
			timerToken = new();
			_ = EstimatedTimer(timerToken.Token);
		}

		public void Start()
		{
			timerToken?.Dispose();
			timerToken = new();
			_ = Timer(timerToken.Token);
		}

		public void Stop()
		{
			timerToken?.Cancel();
			timerToken?.Dispose();
			timerToken = null;
		}

		public void Pause()
		{
			isPaused = true;
		}

		public void UnPause()
		{
			isPaused = false;
		}

		private async UniTask EstimatedTimer(CancellationToken cancellation)
		{
			while (ticks > 0)
			{
				while (isPaused)
				{
					await UniTask.Yield(cancellation);
				}

				ticks -= Time.deltaTime;
				TimeSpan span = TimeSpan.FromSeconds(ticks);
				if (span.TotalSeconds <= 0)
				{
					ticks = 0;
					onTimeOut?.Invoke();
				}

				await UniTask.Yield(cancellation);
			}

			Stop();
		}

		private async UniTask Timer(CancellationToken cancellation)
		{
			while (true)
			{
				while (isPaused)
				{
					await UniTask.Yield(cancellation);
				}

				ticks += Time.deltaTime;

				await UniTask.Yield(cancellation);
			}
		}
	}
}