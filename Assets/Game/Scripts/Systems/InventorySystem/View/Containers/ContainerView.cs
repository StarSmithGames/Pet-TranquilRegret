using System.Collections.Generic;

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
			model = new(GetItems());
		}

		public abstract ItemModel[] GetItems();
	}
}