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
		public AssetReferenceT< T > reference;

		public bool IsLoaded => _asset != null;
		
		public T Asset => _asset;
		private T _asset;

		public void Load( Action callback = null )
		{
			LoadAsync( callback ).Forget();
		}
		
		public T Load()
		{
			if ( _asset == null )
			{
				var operation = reference.LoadAssetAsync();
				_asset = operation.WaitForCompletion();
			}

			return _asset;
		}

		public async UniTask LoadAsync( Action callback = null )
		{
			if ( _asset != null ) return;
			
			Debug.Log( $"[Asset] Start loading {reference.AssetGUID}" );
			
			var handle = reference.LoadAssetAsync();
			await handle;
			_asset = handle.Result;
			
			Debug.Log( $"[Asset] Asset loaded {reference.AssetGUID} : {Asset.name}" );

			callback?.Invoke();
		}
		
		public void Release()
		{
			
		}
	}
}