using Game.Services.TickableService;
using System;
using UnityEngine;

namespace Game.Systems.BoosterManager
{
	public abstract class InfinityBooster : Booster
	{
		public event Action OnStarted;
		public event Action OnStopped;
		public event Action OnTicked;
		
		public bool IsInProcess { get; protected set; }
		
		protected float _ticks = 0;
		
		private readonly TickableService _tickableService;

		public InfinityBooster(
			TickableService tickableService
			)
		{
			_tickableService = tickableService ?? throw new ArgumentNullException( nameof(tickableService) );
		}

		public abstract float GetProgress();
		
		public abstract float GetTicks();
		
		protected virtual void StartInfinity()
		{
			IsInProcess = true;
			_tickableService.OnUpdate += Tick;
			
			OnStarted?.Invoke();
		}

		protected virtual void StopInfinity()
		{
			_tickableService.OnUpdate -= Tick;
			_ticks = 0;
			IsInProcess = false;
			
			OnStopped?.Invoke();
		}

		protected virtual void Tick()
		{
			_ticks += Time.deltaTime;

			if ( IsTick() )
			{
				OnTicked?.Invoke();
			}
			else
			{
				StopInfinity();
			}
		}

		protected abstract bool IsTick();
	}
}