using Game.Managers.PauseManager;
using StarSmithGames.Core.Utils;
using System;
using System.Threading;

namespace Game.Systems.LevelSystem
{
	public abstract class Timer : IPausable
	{
		protected bool _isPaused;
		public float RemainingTime => _remainingTime;
		protected float _remainingTime;
		protected CancellationTokenSource _timerToken;
		
		public void Stop()
		{
			_timerToken?.Cancel();
			_timerToken?.Dispose();
			_timerToken = null;
		}
		
		public void Pause() => _isPaused = true;

		public void UnPause() => _isPaused = false;
		
		public string GetTime()
		{
			return TimeUtils.GetTimerFormatHMS(TimeSpan.FromSeconds(_remainingTime));
		}
		
		public int GetMinutes()
		{
			return TimeSpan.FromSeconds(_remainingTime).Minutes;
		}

		public int GetSeconds()
		{
			return TimeSpan.FromSeconds(_remainingTime).Seconds;
		}

		public int GetMilliseconds()
		{
			return TimeSpan.FromSeconds(_remainingTime).Milliseconds / 10;
		}
	}
}