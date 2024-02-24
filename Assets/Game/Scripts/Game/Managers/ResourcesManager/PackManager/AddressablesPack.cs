using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Managers.AssetManager.PackManager
{
	public class AddressablesPack
	{
		protected AsyncOperationHandle _asyncOperationHandle;
		protected AsyncOperationHandle _downloadSizeHandle;
		
		protected PackState _packState;
		protected PackSource _packSource;
		private string _label;
		
		protected bool _applicationBeenClosed;
		protected DateTime _loadingStartTime;
		protected DateTime _currentLoadingTime;

		public AddressablesPack( string label )
		{
			_label = label;
		}
		
		public async UniTask Load( PackSource source )
		{
			_packSource = source;
			_loadingStartTime = DateTime.Now;
			_currentLoadingTime = DateTime.Now;
			
			await LoadInternal( _packSource );
		}
		
		private async UniTask LoadInternal( PackSource source )
		{
			Debug.Log( $"[PackManager] Load {_label}" );
            _packState = PackState.Loading;

            float size = -1;
            
            _downloadSizeHandle = Addressables.GetDownloadSizeAsync( _label );
            _downloadSizeHandle.Completed += completeSize =>
            {
                if (completeSize.Status == AsyncOperationStatus.Succeeded)
                {
                    var bytes = (long)completeSize.Result;
                    size = bytes;
                    
                    Debug.Log( $"[PackManager] Complete with size:{size}" );
                }
                else
                {
	                Debug.LogError( $"[PackManager] Complete with error: {completeSize.OperationException.Message}" );
                }
            };

            await _downloadSizeHandle;

            if ( size <= 0 )
            {
	            Debug.Log( $"[PackManager] Pack Cached {_label}" );
	            return;
            }

            _asyncOperationHandle = Addressables.DownloadDependenciesAsync( _label );
            _asyncOperationHandle.Completed += complete =>
            {
                if (complete.IsDone)
                {
	                Debug.Log($"[PackManager] Load done:{_label} status:{complete.Status}");

                    if (complete.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (_packState != PackState.Load)
                        {
                            _packState = PackState.Load;
                            
                            if (!_applicationBeenClosed)
                            {
                                _currentLoadingTime = DateTime.Now;
                            }
                        }
                    }
                    else
                    {
                        var error = complete.OperationException;
                        if (complete.OperationException != null)
                        {
                            // _lastLoadError = error.Message;

                            Debug.LogError($"[PackManager] Error downloading settings data: {error.Message}");
                            Debug.LogError($"[PackManager] Error downloading settings data: {error.GetBaseException().Message}");
                            Debug.LogError($"[PackManager] Error downloading settings data: {error.StackTrace}");
                            
                            if (error is IOException)
                            {
                                // _signalBus.Fire(new SpaceErrorSignal());
                            }
                            else if (error is SocketException || error is WebException)
                            {
                                // _signalBus.Fire(new NetworkErrorSignal());
                            }
                        }
                        Debug.LogError($"[PackManager] Load failed {_label}");

                        // LogSystem.Debug($"[LevelPack] Load failed {_preloadLabelname}");
                        //LogSystem.Debug($"!!!!!!AddressablesLevelPack {_preloadLabelname} reload after 10 sec.");
                        // _signalBus.Fire(new LoadedLevelPackSignal { Number = _number, PackSource = source, LevelPackType = LoadingPackWindow.LevelPackType.LineLevels});
                        _packState = PackState.NeedReloading;

                        //DOVirtual.DelayedCall(10f, LoadInternal);
                    }
                }
                else
                {
                    // _lastLoadError = "load undone";
                    Debug.LogError($"[PackManager] Load undone {_label}");
                    _packState = PackState.NeedReloading;
                }
            };
            await _asyncOperationHandle;
		}
	}
}