using Game.Systems.CameraSystem;
using Game.Systems.LockpickingSystem;

using UnityEngine;

namespace Game.Entities
{
	public class PlayerCanvas : CameraTracker
	{
		[field: SerializeField] public UILockpick Lockpick { get; private set; }
	}
}