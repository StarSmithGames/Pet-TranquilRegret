using Game.Services;
using Game.Services.TickableService;
using Game.Systems.BoosterManager.Settings;
using Game.Systems.LevelSystem;
using System;

namespace Game.Systems.BoosterManager
{
	public sealed class VisionBooster : InfinityBooster
	{
		private ILevel _level;

		private readonly VisionBoosterSettings _settings;
		private readonly OutlinableService _outlinableService;
		
		public VisionBooster(
			TickableService tickableService,
			
			VisionBoosterSettings settings,
			OutlinableService outlinableService
			) : base(tickableService)
		{
			_settings = settings ?? throw new ArgumentNullException( nameof(settings) );
			_outlinableService = outlinableService ?? throw new ArgumentNullException( nameof(outlinableService) );
		}
		
		public void Use( ILevel level )
		{
			if ( IsInProcess ) return;

			_level = level;
			
			StartInfinity();
		}

		protected override void StartInfinity()
		{
			base.StartInfinity();

			_outlinableService.SetOutlineData( _settings.Outline );
			_outlinableService.Enable( true );
		}

		protected override void StopInfinity()
		{
			_outlinableService.Enable( false );
			
			base.StopInfinity();
		}

		public override float GetTicks() => _settings.EstimatedTime - _ticks;

		public override float GetProgress() => 1f - ( _ticks / _settings.EstimatedTime );
		
		protected override bool IsTick() => _ticks <= _settings.EstimatedTime;
	}
}