using Game.Environment.PickupableSystem;
using Game.Systems.CameraSystem;

using UnityEngine;

namespace Game.Entity.CharacterSystem
{
	public class ItemCanvas : CameraTracker
	{
		[field: SerializeField] public UIPickup Pickup { get; private set; }
	}
}