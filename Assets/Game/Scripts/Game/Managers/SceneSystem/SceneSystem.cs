using Cysharp.Threading.Tasks;
using StarSmithGames.Go.SceneManager;
using StarSmithGames.IoC.AsyncManager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

namespace Game.Systems.SceneSystem
{
	public class SceneSystem
	{
		public IProgressHandler ProgressHandler { get; private set; }

        private Scene _currentScene;

        public SceneSystem()
        {
            _currentScene = GetActiveScene();
        }
        
        public Scene GetActiveScene()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }
        
        public void LoadSceneForce(int scene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }
        
        #region Load From Build
        public async UniTask LoadSceneAsyncFromBuild(string sceneName, bool allow)
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
            await LoadFromBuild(scene.buildIndex, allow);
        }
        
        public async UniTask LoadSceneAsyncFromBuild(int sceneBuildIndex, bool allow)
        {
            await LoadFromBuild(sceneBuildIndex, allow);
        }

        private async UniTask LoadFromBuild(int sceneBuildIndex, bool allow)
        {
            BuildProgressHandler handle = new();
            ProgressHandler = handle;
        
            handle.asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single);
            handle.asyncOperation.allowSceneActivation = allow;
            await handle.asyncOperation;
        
            if (handle.asyncOperation.isDone)
            {
                _currentScene = GetActiveScene();
            }
            else
            {
                throw new SceneNoLoadedException( $"[{GetType().Name}] Can not load scene {sceneBuildIndex}!" );
            }
        }
        #endregion

        #region Load From Addressables
        
        private Dictionary<string, IResourceLocation> _resourceLocations = new();

        public async UniTask LoadSceneFromAddressables( string locationKey, string addressableLocationKey, bool allow )
        {
            await LoadFromAddressables( locationKey, addressableLocationKey, allow );
        }
        
        private async UniTask LoadFromAddressables( string locationKey, string addressableLocationKey, bool allow )
        {
            AddressablesProgressHandler handle = new();
            ProgressHandler = handle;
            
            IResourceLocation location = null;
            await LoadLocation(locationKey, addressableLocationKey, (x) =>
            {
                location = x;
            });
            
            if (location != null)
            {
	            Debug.Log($"[SceneManager] Start DownloadDependenciesAsync {addressableLocationKey}");
	            handle.dependenciesHandle = Addressables.DownloadDependenciesAsync( addressableLocationKey );
	            await handle.dependenciesHandle;
	            Debug.Log($"[SceneManager] End DownloadDependenciesAsync");
				
                Debug.Log($"[SceneManager] Start LoadSceneAsync {location}");
                handle.sceneHandle = Addressables.LoadSceneAsync( location );
                await handle.sceneHandle;
                Debug.Log($"[SceneManager] End LoadSceneAsync");

                if (handle.sceneHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    _currentScene = GetActiveScene();

                    if ( !allow )
                    {
                        await handle.sceneHandle.Result.ActivateAsync();
                    }
                }
                else if (handle.sceneHandle.Status == AsyncOperationStatus.Failed)
                {
                    throw new SceneNoLoadedException($"[SceneManager] Can't load location by key: {locationKey} with addressable: {addressableLocationKey} Exception: {handle.sceneHandle.OperationException}");
                }
                else
                {
                    throw new SceneNoLoadedException($"[SceneManager] Can't load location by key: {locationKey} with addressable: {addressableLocationKey}, incorrect status");
                }
            }
            else
            {
                throw new SceneNoLoadedException($"[SceneManager] Can't load location by key: {locationKey} with addressable: {addressableLocationKey}, location == NULL");
            }
        }

        private async UniTask LoadLocation( string locationKey, string addressableLocationKey, Action< IResourceLocation > callback )
        {
            if ( !_resourceLocations.TryGetValue( locationKey, out var location ) )
            {
                var addressablesHandle = ProgressHandler as AddressablesProgressHandler;

                addressablesHandle.locationHandle = Addressables.LoadResourceLocationsAsync( addressableLocationKey );
                await addressablesHandle.locationHandle;

                if ( addressablesHandle.locationHandle.IsDone && addressablesHandle.locationHandle.Status == AsyncOperationStatus.Succeeded && addressablesHandle.locationHandle.Result.Count > 0 )
                {
                    location = addressablesHandle.locationHandle.Result[ 0 ];
                    if ( !_resourceLocations.ContainsKey( locationKey ) )
                    {
                        _resourceLocations.Add( locationKey, location );
                    }
                    else
                    {
                        _resourceLocations[ locationKey ] = location;
                    }

                }
            }

            if ( location == null )
            {
                throw new SceneNoLoadedException( $"[SceneManager] Can't load location by key: {locationKey} with addressable: {addressableLocationKey}" );
            }

            callback.Invoke( location );
        }

        #endregion
	}
	
	public class SceneNoLoadedException : Exception
	{
		public SceneNoLoadedException( string message ) : base( message )
		{
            
		}
	}
}