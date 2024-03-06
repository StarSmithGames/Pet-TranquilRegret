using System;
using UnityEngine;

namespace Game.Extensions
{
	public class JointComponent : MonoBehaviour
	{
		public event Action<float> onJointBreaked;

		private void OnJointBreak(float breakForce)
		{
			onJointBreaked?.Invoke(breakForce);
		}
	}
}