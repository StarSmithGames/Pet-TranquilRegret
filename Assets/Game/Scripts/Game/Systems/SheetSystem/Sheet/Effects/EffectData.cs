using Game.Entities;
using Sirenix.OdinInspector;

using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.SheetSystem.Effects
{
	public class EffectData : ScriptableObject
	{
		[HideLabel]
		public Information information;

		//[SerializeReference] public List<EnchantmentType> enchantments = new List<EnchantmentType>();
	}
}