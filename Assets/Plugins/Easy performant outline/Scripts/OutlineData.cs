using EPOOutline;
using Sirenix.OdinInspector;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPOOutline
{
	[CreateAssetMenu(fileName = "OutlineData", menuName = "Game/Outline")]
	public class OutlineData : ScriptableObject
	{
		public OutlineType outlineType;
		[HideLabel]
		public Outlinable.Settings settings;
	}

	public enum OutlineType : int
	{
		Character = 0,
		Enemy = 1,

		Object = 10,

		Selected = 20,

		Target = 30,
	}
}