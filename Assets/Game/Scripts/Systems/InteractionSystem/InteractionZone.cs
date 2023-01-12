using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.InteractionSystem
{
    public class InteractionZone : MonoBehaviour
    {
		public event UnityAction onCollectionChanged;

		[SerializeField] private Settings settings;

		protected List<Collider> collectionList = new List<Collider>();

		private void OnTriggerEnter(Collider other)
		{
			if (settings.layer != (settings.layer | (1 << other.gameObject.layer))) return;

			if (!collectionList.Contains(other))
			{
				collectionList.Add(other);
				onCollectionChanged?.Invoke();

				OnEnter(other);
				OnCollectionChanged();
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (settings.layer != (settings.layer | (1 << other.gameObject.layer))) return;

			if (collectionList.Contains(other))
			{
				collectionList.Remove(other);
				onCollectionChanged?.Invoke();

				OnExit(other);
				OnCollectionChanged();
			}
		}

		public List<Collider> GetCollection()
		{
			return collectionList;
		}

		protected virtual void OnEnter(Collider other)
		{

		}

		protected virtual void OnExit(Collider other)
		{

		}

		protected virtual void OnCollectionChanged()
		{

		}

		[System.Serializable]
		public class Settings
		{
			public LayerMask layer;
		}
	}
}