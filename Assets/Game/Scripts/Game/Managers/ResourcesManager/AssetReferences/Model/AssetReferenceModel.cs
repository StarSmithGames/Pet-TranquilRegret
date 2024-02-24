using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Game.Managers.AssetManager.AssetReferences
{
	[ System.Serializable ]
	public class AssetReferenceModel< T >
		where T : Object
	{
		public bool IsLoaded => reference.OperationHandle.IsValid() && reference.OperationHandle.IsDone;
		
		public AssetReferenceT< T > reference;
		
		public T Asset => IsLoaded ? reference.OperationHandle.Result as T : null;

		public void Load( Action callback = null )
		{
			LoadAsync( callback ).Forget();
		}

		public async UniTask LoadAsync( Action callback = null )
		{
			if ( IsLoaded ) return;
			if ( !reference.IsValid() )
			{
				Debug.LogError( $"[Asset] Not Valid" );
				return;
			}
			
			await reference.LoadAssetAsync();
			callback?.Invoke();
		}
		
		public void Release()
		{
			
		}
	}
}