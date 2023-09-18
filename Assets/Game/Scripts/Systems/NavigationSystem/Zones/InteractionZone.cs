using Sirenix.OdinInspector;

using UnityEngine;

using StarSmithGames.Core;
using Game.VFX;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Systems.NavigationSystem
{
	public class InteractionZone : MonoBehaviour, IEnableable
	{
		public Registrator<Collider> Registrator { get; } = new();

		public bool IsEnable { get; private set; } = true;

		public DecalVFX decal;

		[InfoBox("Required Trigger Collider", InfoMessageType.Error, VisibleIf = "IsColliderTrigger")]
		[SerializeField] private Settings settings;

		private void OnTriggerEnter(Collider other)
		{
			if (!IsEnable) return;

			if (settings.layer != (settings.layer | (1 << other.gameObject.layer))) return;

			if (!Registrator.registers.Contains(other))
			{
				Registrator.Registrate(other);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (!IsEnable) return;

			if (settings.layer != (settings.layer | (1 << other.gameObject.layer))) return;

			if (Registrator.registers.Contains(other))
			{
				Registrator.UnRegistrate(other);
			}
		}

		public void Enable(bool trigger)
		{
			IsEnable = trigger;

			if (!trigger)
			{
				Registrator.registers.Clear();
			}
		}

		public virtual void DoIdle()
		{
			decal?.DoIdle();
		}

		public virtual void DoEnter()
		{
			decal?.DoKill();
			decal?.ScaleTo(1.2f);
		}

		public virtual void ResetAnimation()
		{
			decal?.ScaleTo(1f, callback: () => decal.DoIdle());
		}

#if UNITY_EDITOR
		private bool IsColliderTrigger()
		{
			return !GetComponentInChildren<Collider>()?.isTrigger ?? false;
		}

		private void OnDrawGizmosSelected()
		{
			if (IsColliderTrigger()) return;

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