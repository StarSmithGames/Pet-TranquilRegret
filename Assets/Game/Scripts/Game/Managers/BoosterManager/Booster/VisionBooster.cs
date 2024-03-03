using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Services;
using Game.Services.TickableService;
using Game.Systems.BoosterManager.Settings;
using Game.Systems.StorageSystem;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Systems.BoosterManager
{
	public sealed class VisionBooster : InfinityBooster
	{
		private CancellationTokenSource _cancellationTokenSource;

		private readonly VisionBoosterSettings _settings;
		private readonly OutlinableService _outlinableService;
		private readonly StorageSystem.StorageSystem _storageSystem;
		
		public VisionBooster(
			TickableService tickableService,
			
			BoosterSettings settings,
			OutlinableService outlinableService,
			StorageSystem.StorageSystem storageSystem
			) : base(tickableService)
		{
			_settings = settings?.VisionBoosterSettings ?? throw new ArgumentNullException( nameof(settings) );
			_outlinableService = outlinableService ?? throw new ArgumentNullException( nameof(outlinableService) );
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
			base.StartInfinity();

			var copy = ScriptableObject.Instantiate( _settings.Outline );
			
			_outlinableService.SetOutlineData( copy );
			_outlinableService.Enable( true );

			_cancellationTokenSource = new();
			Idle( _cancellationTokenSource.Token ).Forget();
		}

		protected override void StopInfinity()
		{
			_outlinableService.Enable( false );
			
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = null;
			
			base.StopInfinity();
		}

		private async UniTask Idle( CancellationToken token )
		{
			bool isCanceled = false;
			
			_outlinableService.Fade( 0 );
			
			while ( !isCanceled )
			{
				isCanceled = await _outlinableService.DOFadeColorFill( 0.5f, 0.66f, Ease.InOutSine, token ).SuppressCancellationThrow();
				if ( isCanceled ) break;
				isCanceled = await _outlinableService.DOFadeColorFill( 0f, 0.66f, Ease.InOutSine, token ).SuppressCancellationThrow();
			}
			
			await _outlinableService.DOFadeColorFill( 0f, 0.33f, Ease.OutQuart );
		}

		public override float GetTicks() => _settings.EstimatedTime - _ticks;

		public override float GetProgress() => 1f - ( _ticks / _settings.EstimatedTime );
		
		protected override bool IsTick() => _ticks <= _settings.EstimatedTime;
		
		protected override BoosterData GetData() => _storageSystem.Storage.VisionBooster.Value;
	}
}