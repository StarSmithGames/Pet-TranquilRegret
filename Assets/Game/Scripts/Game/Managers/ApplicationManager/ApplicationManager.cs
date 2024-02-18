using Game.Systems.StorageSystem;
using StarSmithGames.Core.Utils;
using StarSmithGames.Go.ApplicationHandler;
using System;
using Zenject;

namespace Game.Managers.ApplicationManager
{
	public sealed class ApplicationManager : IInitializable, IDisposable
	{
		private bool _isFirstTime;
		
		private SignalBus _signalBus;
		private StorageSystem _storageSystem;
		private GameManager.GameManager _gameManager;
		
		public ApplicationManager(
			SignalBus signalBus,
			StorageSystem storageSystem,
			GameManager.GameManager gameManager
			)
		{
			_signalBus = signalBus;
			_storageSystem = storageSystem;
			_gameManager = gameManager;
			
			_isFirstTime = _storageSystem.GameFastData.IsFirstTime;
		}
		
		public void Initialize()
		{
			
			CheckInterruptGameProcess();
			
			_signalBus?.Subscribe<SignalOnApplicationFocusChanged>(ApplicationChangedHandler);
		}

		public void Dispose()
		{
			_signalBus?.Unsubscribe<SignalOnApplicationFocusChanged>(ApplicationChangedHandler);
		}
		
		private void CheckInterruptGameProcess()
		{
			CloseData closeParams = _storageSystem.GameFastData.CloseParams.GetData();

			//if (closeParams.IsPromptValid)
			//{
			//	_analyticsSystem.push_custom(closeParams.promptLevelFinishName, closeParams.promptLevelFinish);
			//}
		}
		
		private void ApplicationChangedHandler( SignalOnApplicationFocusChanged signal )
		{
			if (!signal.trigger)
			{
				_storageSystem.GameFastData.IsFirstTime = false;

				var data = new CloseData
				{
					//appCloseTimestampUTC = TimestampUtils.GetTimestamp(_ntpDateTimeService.Now.ToUniversalTime()),
					appCloseTimestampLocal = TimestampUtils.GetTimestamp(DateTime.Now.ToLocalTime()),
					isInterruptGameProcess = false
				};

				if ( _gameManager.IsGame )
				{
					// if (!viewService.IsSafeDialogShowig())
					{
						//var level = levelManager.CurrentLevel;
						//var isSafeLevel = !level.Model.UseLives;// || level.Model.UseLives && _livesManager.IsInfiniteLives();
						//data.isInterruptGameProcess = !isSafeLevel;
					}

					//var items = storageSystem.IntermediateData.CurrentLevel.GetPromtInterruptedGameProccess();
					//data.promptLevelFinishName = items.Item1;
					//data.promptLevelFinish = items.Item2;
				}

				_storageSystem.GameFastData.CloseParams.SetData(data);

				_storageSystem.Save(); //required
			}
			else
			{
				if ( _isFirstTime )
				{
					_storageSystem.GameFastData.IsFirstTime = true;
				}
			}
		}
	}
}