using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Game.Managers.AssetManager.AssetReferences
{
	public abstract class AssetReferenceModel< T >
		where T : Object
	{
		public event Action OnAssetLoaded;
		
		public bool IsLoaded => reference.OperationHandle.IsValid() && reference.OperationHandle.IsDone;
		
		public AssetReferenceT< T > reference;
		
		public T Asset => IsLoaded ? reference.OperationHandle.Result as T : null;

		public void Load()
		{
			if ( IsLoaded ) return;

			if ( !reference.IsValid() )
			{
				Debug.LogError( "[Asset] Not Valid" );
				return;
			}
			LoadAssetAsync();
		}

		public void Release()
		{
			
		}

		private async UniTask LoadAssetAsync()
		{
			await reference.LoadAssetAsync();
			OnAssetLoaded?.Invoke();
		}
	}
}