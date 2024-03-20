using Game.Managers.LayerManager;
using Game.Systems.ZoneSystem;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Systems.InteractionSystem
{
#if UNITY_EDITOR
	[ExecuteAlways]
#endif
	public abstract class InteractionZone<T> : Zone
		where T : IZonable
	{
		public event Action< T > OnItemAdded;
		public event Action< T > OnItemRemoved;

		public override Collider Collider => collider;
		public SphereCollider collider;
#if UNITY_EDITOR
		public bool isDebugSetup = true;
#endif

		protected override void ItemAddedHandler(IZonable item)
		{
			OnItemAdded?.Invoke((T)item);
		}

		protected override void ItemRemovedHandler(IZonable item)
		{
			OnItemRemoved?.Invoke((T)item);
		}

#if UNITY_EDITOR
		private void Update()
		{
			if (Application.isPlaying) return;

			if (collider == null) return;

			if (!isDebugSetup) return;

			Ray ray = new(transform.position + Vector3.up * 10, Vector3.down);
			if (Physics.Raycast(ray, out var hit, 100, Layers.LAYER_GROUND))
			{
				collider.center = transform.InverseTransformPoint(hit.point);
			}
			else
			{
				collider.center = Vector3.zero;
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (collider == null) return;

			if (!collider.isTrigger) return;

			var style = new GUIStyle();
			style.normal.textColor = Color.green;

			Handles.Label(transform.position + (Vector3.right * collider.radius), gameObject.name, style);
		}
#endif
	}
}