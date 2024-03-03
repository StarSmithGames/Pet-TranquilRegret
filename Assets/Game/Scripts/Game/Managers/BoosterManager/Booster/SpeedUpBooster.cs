using Game.Services.TickableService;
using Game.Systems.BoosterManager.Settings;
using System;

namespace Game.Systems.BoosterManager
{
	public sealed class SpeedUpBooster : InfinityBooster
	{
		private SpeedUpBoosterSettings _settings;
		
		public SpeedUpBooster(
			TickableService tickableService,
			
			SpeedUpBoosterSettings settings
			) : base( tickableService )
		{
			_settings = settings ?? throw new ArgumentNullException( nameof(settings) );
		}
		
		public void Use()
		{
			if ( IsInProcess ) return;

			StartInfinity();
		}

		protected override void StartInfinity()
		{
			base.StartInfinity();
		}

		protected override void StopInfinity()
		{
			base.StopInfinity();
		}

		public override float GetTicks() => _settings.EstimatedTime - _ticks;

		public override float GetProgress() => 1f - ( _ticks / _settings.EstimatedTime );
		
		protected override bool IsTick() => _ticks <= _settings.EstimatedTime;
	}
}