using UnityEngine;
using Game.Managers.LayerManager;
using Game.Systems.InteractionSystem;

namespace Game.Systems.NavigationSystem
{
	[AddComponentMenu("AGame/Area/Character Interaction Zone")]
	public class CharacterInteractionZone : InteractionZone<Entity.CharacterSystem.Character>
	{	
		protected override void OnTriggerEnter(Collider other)
		{
			if (!Layers.IsContains(Layers.LAYER_CHARACTER, other.gameObject.layer)) return;

			base.OnTriggerEnter(other);
		}

		protected override void OnTriggerExit(Collider other)
		{
			if (!Layers.IsContains(Layers.LAYER_CHARACTER, other.gameObject.layer)) return;

			base.OnTriggerExit(other);
		}
	}
}