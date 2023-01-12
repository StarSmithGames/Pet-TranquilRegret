using Game.Entities;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.InteractionSystem
{
	public class EffectZone : InteractionZone
	{
		[SerializeReference] private Effect effect;

		protected override void OnEnter(Collider other)
		{
			var character = other.GetComponentInParent<ICharacter>();
			if(character != null)
			{
				character.Effects.Registrate(effect);
			}
		}

		protected override void OnExit(Collider other)
		{
			var character = other.GetComponentInParent<ICharacter>();
			if (character != null)
			{
				character.Effects.UnRegistrate(effect);
			}
		}
	}

	public abstract class Effect
	{

	}

	public class IncreaseJumpImpulse : Effect
	{
		public Vector3 jumpImpulse = new Vector3(0, 10, 0);
	}

	public class Effects : Registrator<Effect>
	{
		private ICharacter character;

		public Effects(ICharacter character)
		{
			this.character = character;
		}

		public Vector3 GetJumpImpulse()
		{
			if (ContainsType<IncreaseJumpImpulse>())
			{
				return GetAs<IncreaseJumpImpulse>().jumpImpulse;
			}

			return Vector3.zero;
		}

		protected override void OnRegistrated(Effect effect)
		{
			
		}

	}
}