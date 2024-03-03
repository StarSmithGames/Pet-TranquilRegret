using Game.Managers.CharacterManager;
using Game.Services.TickableService;
using Game.Systems.BoosterManager.Settings;
using Game.Systems.SheetSystem.Effects;
using Game.Systems.StorageSystem;
using System;

namespace Game.Systems.BoosterManager
{
	public sealed class SpeedUpBooster : InfinityBooster
	{
		private AddMoveSpeed _effect;
		
		private readonly SpeedUpBoosterSettings _settings;
		private readonly CharacterManager _characterManager;
		private readonly StorageSystem.StorageSystem _storageSystem;
		
		public SpeedUpBooster(
			TickableService tickableService,
			
			BoosterSettings settings,
			CharacterManager characterManager,
			StorageSystem.StorageSystem storageSystem
			) : base( tickableService )
		{
			_settings = settings?.SpeedUpBoosterSettings ?? throw new ArgumentNullException( nameof(settings) );
			_characterManager = characterManager ?? throw new ArgumentNullException( nameof(characterManager) );
			_storageSystem = storageSystem ?? throw new ArgumentNullException( nameof(storageSystem) );
		}
		
		public void Use()
		{
			if ( IsInProcess ) return;

			Data.ItemsCount--;
			
			StartInfinity();
		}

		protected override void StartInfinity()
		{
			var sheet = _characterManager.Player.Presenter.Model.Sheet;
			_effect = new AddMoveSpeed( sheet.MoveSpeed, _settings.SpeedMultiplier );
			_effect.Activate();
			
			base.StartInfinity();
		}

		protected override void StopInfinity()
		{
			_effect.Deactivate();
			_effect = null;
			
			base.StopInfinity();
		}

		public override float GetTicks() => _settings.EstimatedTime - _ticks;

		public override float GetProgress() => 1f - ( _ticks / _settings.EstimatedTime );
		
		protected override bool IsTick() => _ticks <= _settings.EstimatedTime;

		protected override BoosterData GetData() => _storageSystem.Storage.SpeedUpBooster.Value;
	}
}