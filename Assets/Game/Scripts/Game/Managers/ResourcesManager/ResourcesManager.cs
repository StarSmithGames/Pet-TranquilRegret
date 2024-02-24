using Cysharp.Threading.Tasks;
using Game.Managers.AssetManager.PackManager;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = System.Object;

namespace Game.Managers.AssetManager
{
	public sealed class ResourcesManager
	{
		public event Action OnAddressablesInitialized;
		
		public bool IsAddressablesInitialized { get; private set; }
		
		public ResourcesManager()
		{
			UnityEngine.ResourceManagement.ResourceManager.ExceptionHandler += ExceptionHandler;
			
			InitializeAddressables().Forget();
		}

		private async UniTask InitializeAddressables()
		{
			await Addressables.InitializeAsync();
			IsAddressablesInitialized = true;
			OnAddressablesInitialized?.Invoke();

			// await DownloadDependenciesAsync( "character" );
			// await new AddressablesPack( "characters" ).Load( PackSource.Local );
			// await new AddressablesLevelPack( 1 ).Load( PackSource.Local );
		}
		
		public async UniTask DownloadDependenciesAsync( string label )
		{
			var downloadHandle = Addressables.DownloadDependenciesAsync( label );
			downloadHandle.Completed += complete =>
			{
				if (complete.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
				{
					Debug.Log( $"[ResourcesManager] Asset loaded:{label} status:{complete.Status}" );
				}
				else
				{
					Debug.LogError( $"[ResourcesManager] Asset undone:{label} status:{complete.Status}" );
				}
			};

			await downloadHandle;
		}

		public async UniTask< Object > RequestResource( bool isInternal, Type type, string name )
		{
			if ( !isInternal )
			{
				// InternetConnectionStatus internetConnectionStatus = await _networkStatusProvider.CheckConnection();
				// if ( internetConnectionStatus != InternetConnectionStatus.Connected )
				// {
				// 	SendAnalyticsError( analytics, name, 0 );
				// 	return default;
				// }
			}

			float time = Time.realtimeSinceStartup;
			try
			{
				Object result = await LoadAsync( type, name );
				time = Time.realtimeSinceStartup - time;

				return result;
			}
			catch ( Exception e )
			{
				time = Time.realtimeSinceStartup - time;
				Debug.LogError( $"Error load {e.Message}" );
			}

			return default;
		}
		

		private async UniTask< Object > LoadAsync( Type type, string name )
		{
			if ( type == typeof(Sprite) )
				return await LoadingSpriteAsync( name );
			return await LoadingGameObjectAsync( name );
		}
		private async UniTask< Sprite > LoadingSpriteAsync( string name ) => await Addressables.LoadAssetAsync< Sprite >( name ).ToUniTask();
		private async UniTask< GameObject > LoadingGameObjectAsync( string name ) => await Addressables.LoadAssetAsync< GameObject >( name ).ToUniTask();

		private void ExceptionHandler( AsyncOperationHandle handle, Exception exception )
		{
			Debug.LogError( $"[ResourcesManager] Error in {handle} {exception}" );
		}
	}
}