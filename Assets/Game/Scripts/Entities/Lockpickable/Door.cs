using Game.Systems.LockpickingSystem;
using UnityEngine;

namespace Game.Entities
{
    public class Door : LockpickableObject
	{
		[SerializeField] private Rigidbody rigidbody;
        [SerializeField] private HingeJoint hingeJoint;
        [SerializeField] private Collider collider;
		[SerializeField] private Settings doorSettings;

		private float breakForce;

		protected override void Start()
		{
			breakForce = hingeJoint.breakForce;

			if (settings.isLocked)
			{
				rigidbody.freezeRotation = true;
				hingeJoint.breakForce = Mathf.Infinity;
			}

			base.Start();
		}

		private void OnJointBreak(float breakForce)
		{
			collider.material = null;
		}

		protected override void OnLockChanged()
		{
			base.OnLockChanged();

			hingeJoint.breakForce = doorSettings.isBreakable ? breakForce : Mathf.Infinity;
			rigidbody.freezeRotation = false;
		}

		[System.Serializable]
        public class Settings
        {
            public bool isBreakable = true;
            [HideInInspector] public bool isBreaked = false;
        }
    }
}