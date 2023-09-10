using System.Linq;

using UnityEngine;

namespace Game.Systems.LockpickingSystem
{
    public class Door : LockpickableGroup
	{
		[Space]
		[SerializeField] private Rigidbody rigidbody;
        [SerializeField] private HingeJoint hingeJoint;
        [SerializeField] private Collider collider;
		[SerializeField] private Settings doorSettings;

		private float breakForce;

		protected override void Awake()
		{
			base.Awake();

			breakForce = hingeJoint.breakForce;

			if (base.settings.isLocked)
			{
				rigidbody.freezeRotation = true;
				hingeJoint.breakForce = Mathf.Infinity;
			}
		}

		private void OnJointBreak(float breakForce)
		{
			collider.material = null;
		}

		protected override void OnLockChanged(LockpickableObject locker)
		{
			locker.DoUnlock();
			for (int i = 0; i < locks.Count; i++)
			{
				if (locks[i] != locker)
				{
					locks[i].Hide();
				}
			}

			hingeJoint.breakForce = doorSettings.isBreakable ? breakForce : Mathf.Infinity;
			rigidbody.freezeRotation = false;
		}

		[System.Serializable]
        public class Settings
        {
            public bool isBreakable = true;
        }
    }
}