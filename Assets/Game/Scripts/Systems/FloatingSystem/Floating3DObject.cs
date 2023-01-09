using System.Collections;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Systems.FloatingSystem
{
	public abstract class Floating3DObject : MonoBehaviour
	{
		public bool IsHasTarget => target != null;

		[SerializeField] private Rigidbody rigidbody;
		[SerializeField] private Collider collider;
		[SerializeField] private Settings settings;

		protected Transform target;

		public void SetTarget(Transform target)
		{
			this.target = target;

			StartCoroutine(Animation());
		}

		private IEnumerator Animation()
		{
			collider.enabled = false;
			rigidbody.AddForce(Vector3.up * settings.impulseForce, ForceMode.VelocityChange);

			yield return new WaitForSeconds(settings.impulseTime);

			Disable();
			transform.DOScale(0.5f, 0.5f);

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

			OnAnimationCompleted();

			gameObject.SetActive(false);
		}

		private Vector3[] GetCatmullRomPath()
		{
			return new Path(PathType.CatmullRom, new Vector3[] { transform.position, target.position }, 10).wps;
		}

		private void Disable()
		{
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
		}

		protected abstract void OnAnimationCompleted();

		[System.Serializable]
		public class Settings
		{
			public float impulseForce = 10;
			public float impulseTime = 0.15f;
			[Space]
			public float pathSpeed = 0.1f;
		}
	}
}