using Game.Systems.InventorySystem;
using UnityEngine.AddressableAssets;

namespace Game.Managers.AssetManager.AssetReferences
{
	[ System.Serializable ]
	public sealed class AssetReferenceItemConfig : AssetReferenceT< ItemConfig >
	{
		public AssetReferenceItemConfig( string guid ) : base( guid )
		{
		}
	}
}