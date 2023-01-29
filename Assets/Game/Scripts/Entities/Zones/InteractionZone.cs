using Game.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Entities
{
    public class InteractionZone : MonoBehaviour, IEnableable
	{
		public event UnityAction onCollectionChanged;
		public event UnityAction<Collider> onEnterChanged;
		public event UnityAction<Collider> onExitChanged;

		public bool IsEnable { get; private set; } = true;

		[InfoBox("Required Trigger Collider", InfoMessageType.Error, VisibleIf = "CheckCollider")]
		[SerializeField] private Settings settings;

		protected List<Collider> collectionList = new List<Collider>();

		private void OnTriggerEnter(Collider other)
		{
			if (!IsEnable) return;

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
			if (!IsEnable) return;

			if (settings.layer != (settings.layer | (1 << other.gameObject.layer))) return;

			if (collectionList.Contains(other))
			{
				collectionList.Remove(other);
				onCollectionChanged?.Invoke();

				OnExit(other);
				OnCollectionChanged();
			}
		}

		public void Enable(bool trigger)
		{
			IsEnable = trigger;

			if (!trigger)
			{
				collectionList.Clear();
				onCollectionChanged?.Invoke();
				OnCollectionChanged();
			}
		}

		public List<Collider> GetCollection() => collectionList;

		protected virtual void OnEnter(Collider other)
		{
			onEnterChanged?.Invoke(other);
		}

		protected virtual void OnExit(Collider other)
		{
			onExitChanged?.Invoke(other);
		}

		protected virtual void OnCollectionChanged() { }

		private bool CheckCollider()
		{
			return !GetComponentInChildren<Collider>()?.isTrigger ?? false;
		}

#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			if (CheckCollider()) return;

			var collider = GetComponentInChildren<Collider>();

			if (collider is SphereCollider sphereCollider)
			{
				var style = new GUIStyle();
				style.normal.textColor = Color.green;

				Handles.Label(transform.position + (Vector3.right * sphereCollider.radius), gameObject.name, style);
			}
		}
#endif

		[System.Serializable]
		public class Settings
		{
			public LayerMask layer;
		}
	}
}