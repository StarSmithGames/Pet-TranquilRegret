using UnityEngine;

namespace Game.Systems.InventorySystem
{
	public abstract class ContainerView : MonoBehaviour
    {
		public ContainerModel Model
		{
			get
			{
				if(model == null)
				{
					model = new(GetItems());
				}

				return model;
			}
		}
		private ContainerModel model;

		protected void Initialize()
		{
			var items = GetItems();
			model = new( items );
		}

		public abstract ItemModel[] GetItems();
	}
}