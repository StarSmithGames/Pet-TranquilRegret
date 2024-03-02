using EPOOutline;
using UnityEngine;

namespace Game.Systems.InventorySystem
{
	public abstract class ItemView : MonoBehaviour
	{
		public ItemModel model;

		public Outlinable Outlinable;

		public void SetOutlinableAsset( OutlineData data )
		{
			Outlinable.SetData( data );
		}
	}
}