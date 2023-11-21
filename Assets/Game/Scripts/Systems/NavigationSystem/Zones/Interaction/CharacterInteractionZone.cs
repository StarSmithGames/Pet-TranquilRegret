using UnityEngine;

using Game.VFX;
using Game.Managers.LayerManager;
using Game.Systems.InteractionSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Systems.NavigationSystem
{
#if UNITY_EDITOR
	[ExecuteAlways]
#endif
	public class CharacterInteractionZone : InteractionZone<Character.Character>
	{
		public SphereCollider collider;
		public DecalVFX decal;

#if UNITY_EDITOR
		private void Update()
		{
			if (Application.isPlaying) return;

			if (collider == null) return;

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
#endif

		public void DoIdle()
		{
			decal?.DoIdle();
		}

		public void DoEnter()
		{
			decal?.DoKill();
			decal?.ScaleTo(1.2f);
		}

		public void ResetAnimation()
		{
			decal?.ScaleTo(1f, callback: () => decal.DoIdle());
		}

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

		protected override void OnItemAdded(Character.Character item)
		{
			DoEnter();
		}
		protected override void OnItemRemoved(Character.Character item)
		{
			ResetAnimation();
		}

#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			if (!collider?.isTrigger ?? false) return;

			var style = new GUIStyle();
			style.normal.textColor = Color.green;

			Handles.Label(transform.position + (Vector3.right * collider.radius), gameObject.name, style);
		}
#endif
	}
}