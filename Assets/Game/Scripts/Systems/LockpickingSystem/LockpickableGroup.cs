using Sirenix.OdinInspector.Editor;

using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Game.Systems.LockpickingSystem
{
	public class LockpickableGroup : MonoBehaviour
	{
		public LockpickableSettings settings;

		[HideInInspector] public List<LockpickableObject> locks;

		protected virtual void Awake()
		{
			for (int i = 0; i < locks.Count; i++)
			{
				locks[i].onLockChanged += OnLockChanged;
			}
		}

		private void OnDestroy()
		{
			for (int i = 0; i < locks.Count; i++)
			{
				locks[i].onLockChanged -= OnLockChanged;
			}
		}

		protected virtual void OnLockChanged(LockpickableObject locker) { }
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(LockpickableGroup), true)]
	public class LockpickableGroupEditor : OdinEditor
	{
		private LockpickableGroup group;

		protected override void OnEnable()
		{
			base.OnEnable();

			group = target as LockpickableGroup;

			group.locks = group.GetComponentsInChildren<LockpickableObject>().ToList();
			EditorUtility.SetDirty(group);
		}
	}
#endif
}