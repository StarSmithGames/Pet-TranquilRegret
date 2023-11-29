using Game.Character;
using Game.Extensions;
using Game.Systems.NavigationSystem;

using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Game.Systems.LockpickingSystem
{
    public partial class Door : LockpickableObject
	{
		public Settings doorSettings;
		[Space]
		public Rigidbody rigidbody;
        public HingeJoint hingeJoint;
        public Collider collider;

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

		protected void OnDestroy()
		{

		}

		private void OnJointBreak(float breakForce)
		{
			Debug.LogError("BREAKED");
			//collider.material = null;
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
        }
    }
}