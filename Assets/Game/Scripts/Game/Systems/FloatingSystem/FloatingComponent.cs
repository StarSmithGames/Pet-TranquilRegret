using System.Collections;

using Cysharp.Threading.Tasks;

using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;

using Sirenix.OdinInspector;

using StarSmithGames.Core;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.FloatingSystem
{
	public class FloatingComponent : MonoBehaviour
	{
		public Rigidbody rigidbody;
		public Collider collider;
		public Settings settings;

		private void Awake()
		{
			Enable(false);
		}

		private void Enable(bool trigger)
		{
			rigidbody.useGravity = trigger;
			rigidbody.isKinematic = !trigger;
		}

		public async UniTask DoAnimationAsync(Transform target)
		{
			collider.enabled = false;
			Enable(true);
			rigidbody.AddForce(settings.GetForce(), ForceMode.Impulse);
			rigidbody.maxAngularVelocity = Mathf.Infinity;
			rigidbody.AddTorque(settings.GetTorque());

			await UniTask.WaitForSeconds(settings.impulseTime);

			Vector3 velocity = Vector3.zero;

			while (Vector3.Distance(transform.position, target.position) > 0.66f)
			{
				transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, Time.deltaTime * Random.Range(5f, 10f));
				await UniTask.Yield();
			}

			await UniTask.Yield();

			gameObject.SetActive(false);
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