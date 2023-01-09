using System.Collections;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.FloatingSystem
{
	public abstract class Floating3DObject : MonoBehaviour
	{
		public bool IsHasTarget => CurrentTarget != null;

		[SerializeField] private Rigidbody rigidbody;
		[SerializeField] private Collider collider;
		[SerializeField] private Settings settings;

		public Transform CurrentTarget { get; private set; }

		public void SetTarget(Transform target, UnityAction<Floating3DObject> callback = null)
		{
			this.CurrentTarget = target;

			StartCoroutine(Animation(callback));
		}

		private IEnumerator Animation(UnityAction<Floating3DObject> callback = null)
		{
			collider.enabled = false;

			rigidbody.AddForce(settings.GetForce(), ForceMode.Impulse);
			rigidbody.maxAngularVelocity = Mathf.Infinity;
			rigidbody.AddTorque(settings.GetTorque());

			yield return new WaitForSeconds(settings.impulseTime);

			OnAnimationPathStart();

			int pointIndex = 0;
			float t = 0;
			float a = 1;
			var path = GetCatmullRomPath();

			while (pointIndex < path.Length)
			{
				while (Vector3.Distance(transform.position, path[pointIndex]) > 0.1f)
				{
					path = GetCatmullRomPath();

					transform.position = Vector3.MoveTowards(transform.position, path[pointIndex], settings.pathSpeed * a);

					t += Time.deltaTime;
					a += settings.pathSpeed * Time.deltaTime;
					yield return null;
				}

				pointIndex++;

				yield return null;
			}

			callback?.Invoke(this);
			yield return null;
			gameObject.SetActive(false);
		}

		private Vector3[] GetCatmullRomPath()
		{
			return new Path(PathType.CatmullRom, new Vector3[] { transform.position, CurrentTarget.position }, 10).wps;
		}

		private void Disable()
		{
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
		}

		protected virtual void OnAnimationPathStart()
		{
			Disable();
			transform.DOScale(0.5f, 0.5f);
		}

		[System.Serializable]
		public class Settings
		{
			[Header("Impulse")]
			public float impulseTime = 0.2f;
			[Space]
			public Force jumpForce;
			public Direction jumpDirecion;
			[Space]
			public Force rotationForce;
			public Direction rotationDirecion;
			[Header("Path")]
			public float pathSpeed = 0.1f;

			public Vector3 GetForce()
			{
				return jumpDirecion.GetDirection() * jumpForce.GetForce();
			}

			public Vector3 GetTorque()
			{
				return rotationDirecion.GetDirection() * rotationForce.GetForce();
			}

			[System.Serializable]
			public class Force
			{
				public bool isRandomForce = false;
				[HideIf("isRandomForce")]
				public float force = 15f;

				[ShowIf("isRandomForce")]
				public Vector2 forceMinMax;

				public float GetForce()
				{
					if (isRandomForce)
					{
						return forceMinMax.RandomBtw();
					}

					return force;
				}
			}

			[System.Serializable]
			public class Direction
			{
				public bool isRandomDirection = false;
				[HideIf("isRandomDirection")]
				public Vector3 rotationDirection = new Vector3(0, 1, 0);

				[ShowIf("isRandomDirection")]
				public Vector2 axisXMinMax;
				[ShowIf("isRandomDirection")]
				public Vector2 axisYMinMax;
				[ShowIf("isRandomDirection")]
				public Vector2 axisZMinMax;

				public Vector3 GetDirection()
				{
					if (isRandomDirection)
					{
						return new Vector3(axisXMinMax.RandomBtw(), axisYMinMax.RandomBtw(), axisZMinMax.RandomBtw());
					}

					return rotationDirection;
				}
			}
		}
	}
}