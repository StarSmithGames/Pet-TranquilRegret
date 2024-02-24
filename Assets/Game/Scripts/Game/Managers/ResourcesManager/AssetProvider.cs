using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Game.Managers.AssetManager
{
	public class AssetProvider
	{
		private readonly List<AsyncOperationHandle> _handles = new();

		public async UniTask<T> LoadAsset<T>(string address) where T : Object
		{
			AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
			_handles.Add(handle);
			return await handle;
		}
    
		public async UniTask<T> LoadAsset<T>(AssetReference reference) where T : Object
		{
			AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(reference);
			_handles.Add(handle);

			return await handle;
		}

		public AsyncOperationHandle< T > LoadAssetHandle< T >( AssetReference reference ) where T : Object => Addressables.LoadAssetAsync< T >( reference );

		public AsyncOperationHandle< IList< AsyncOperationHandle > > LoadAllHandles( List< AsyncOperationHandle > handles )
			=> Addressables.ResourceManager.CreateGenericGroupOperation( handles, true );

		public async UniTask<IList<T>> LoadAll<T>(AssetLabelReference labelReference, Action<T> callback)
			where T : Object => await Addressables.LoadAssetsAsync(labelReference, callback);

		public async UniTask<IList<T>> LoadAll<T>(IEnumerable<AssetReference> references, Action<T> callback)
			where T : Object
		{
			IList<IResourceLocation> resourceLocations =
				await Addressables.LoadResourceLocationsAsync(references, Addressables.MergeMode.Union, typeof(T));

			Debug.Log($"location - {resourceLocations[0].PrimaryKey}");
			return await Addressables.LoadAssetsAsync(resourceLocations, callback);
		}

		public async UniTask<GameObject> InstantiateAsync(AssetReference reference) =>
			await Addressables.InstantiateAsync(reference);

		public void Clear()
		{
			foreach (AsyncOperationHandle handle in _handles)
			{
				Addressables.Release(handle);
			}

			_handles.Clear();
		}
	}
}