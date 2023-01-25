using Game.Systems.CameraSystem;
using Game.Systems.PickupableSystem;

using UnityEngine;

namespace Game.Entities
{
	public class ItemCanvas : CameraTracker
	{
		[field: SerializeField] public UIPickup Pickup { get; private set; }
	}
}