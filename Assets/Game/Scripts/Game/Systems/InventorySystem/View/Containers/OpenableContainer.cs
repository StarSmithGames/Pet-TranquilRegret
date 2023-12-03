using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.Systems.InventorySystem
{
	[AddComponentMenu("AGame/Inventory/Openable Container")]
	public class OpenableContainer : ContainerView
	{
		public bool isPhysical = false;
		[HideIf("isPhysical")]
		public List<ItemModel> items = new();

		[ShowIf("isPhysical")]
		public List<ItemView> itemViews = new();
		[ShowIf("isPhysical")]
		public bool isItemsPrefabs = false;

		public override ItemModel[] GetItems()
		{
			throw new System.NotImplementedException();
		}

#if UNITY_EDITOR
		[ShowIf("@isPhysical&&!isItemsPrefabs")]
		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			itemViews = GetComponentsInChildren<ItemView>().ToList();
		}
#endif
	}
}