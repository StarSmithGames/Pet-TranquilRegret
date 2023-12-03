using UnityEngine;

namespace Game.Systems.ZoneSystem
{
	public abstract class ZoneView : MonoBehaviour
	{
		public Zone zone;

		protected virtual void Awake()
		{
			zone.Registrator.onCollectionChanged += OnZoneCollectionChanged;
		}

		protected virtual void OnDestroy()
		{
			zone.Registrator.onCollectionChanged -= OnZoneCollectionChanged;
		}

		protected abstract void OnZoneCollectionChanged();
	}
}