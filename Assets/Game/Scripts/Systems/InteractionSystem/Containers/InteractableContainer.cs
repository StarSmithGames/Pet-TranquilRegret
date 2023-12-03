using Game.Systems.FloatingSystem;
using Game.Systems.GoalSystem;

using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Game.Systems.InteractionSystem
{
    public abstract class InteractableContainer : InteractableObject
    {
		public List<InteractableItem> items = new();

#if UNITY_EDITOR
		[Button(DirtyOnClick = true)]
		private void Fill()
		{
			items = GetComponentsInChildren<InteractableItem>().ToList();
		}
#endif
	}
}