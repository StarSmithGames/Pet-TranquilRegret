using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.InteractionSystem
{
    public class InteractionZone : MonoBehaviour
    {
		public event UnityAction onCollectionChanged;

		[SerializeField] private Settings settings;

		private List<Collider> collectionList = new List<Collider>();

		private void OnTriggerEnter(Collider other)
		{
			if (settings.layer != (settings.layer | (1 << other.gameObject.layer))) return;

			if (!collectionList.Contains(other))
			{
				collectionList.Add(other);
				onCollectionChanged?.Invoke();
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (settings.layer != (settings.layer | (1 << other.gameObject.layer))) return;

			if (collectionList.Contains(other))
			{
				collectionList.Remove(other);
				onCollectionChanged?.Invoke();
			}
		}

		public List<Collider> GetCollection()
		{
			return collectionList;
		}

		[System.Serializable]
		public class Settings
		{
			public LayerMask layer;
		}
	}
}