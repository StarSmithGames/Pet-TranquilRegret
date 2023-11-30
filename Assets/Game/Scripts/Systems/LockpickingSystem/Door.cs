using Game.Character;
using Game.Extensions;
using Game.Systems.NavigationSystem;
using Game.Systems.PhysicsSystem;

using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

using Zenject;

namespace Game.Systems.LockpickingSystem
{
    public partial class Door : LockpickableObject
	{
		public Settings doorSettings;
		[Space]
		public Rigidbody rigidbody;
        public HingeJoint hingeJoint;
		public JointComponent joint;
		public Collider collider;

		[Inject] private PhysicsSettings physicsSettings;

		protected override void Awake()
		{
			base.Awake();

			if (base.settings.isLocked)
			{
				hingeJoint.breakForce = Mathf.Infinity;
				rigidbody.freezeRotation = true;
			}
			else
			{
				hingeJoint.breakForce = doorSettings.isBreakable ? doorSettings.breakForce : Mathf.Infinity;
			}

			joint.onJointBreaked += OnJointBreaked;
		}

		protected void OnDestroy()
		{
			joint.onJointBreaked -= OnJointBreaked;
		}

		private void OnJointBreaked(float breakForce)
		{
			collider.material = physicsSettings.frictionMax;
		}

		//protected override void OnLockChanged(LockpickableObject locker)
		//{
		//	locker.DoUnlock();
		//	for (int i = 0; i < locks.Count; i++)
		//	{
		//		if (locks[i] != locker)
		//		{
		//			locks[i].Hide();
		//		}
		//	}

		//	hingeJoint.breakForce = doorSettings.isBreakable ? breakForce : Mathf.Infinity;
		//	rigidbody.freezeRotation = false;
		//}

		[System.Serializable]
        public class Settings
        {
            public bool isBreakable = true;
			public float breakForce = 100f;
		}
    }
}