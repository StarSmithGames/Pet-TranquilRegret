using Game.Entities;
using Game.Systems.SheetSystem.Effects;

using UnityEngine;

namespace Game.Systems.InteractionSystem
{
	public class EffectZone : InteractionZone
	{
		[SerializeField] private EffectData data;

		protected override void OnEnter(Collider other)
		{
			var character = other.GetComponentInParent<ICharacter>();
			if(character != null)
			{
				//character.Sheet.Effects.Registrate(effect);
			}
		}

		protected override void OnExit(Collider other)
		{
			var character = other.GetComponentInParent<ICharacter>();
			if (character != null)
			{
				//character.Sheet.Effects.UnRegistrate(effect);
			}
		}
	}
}