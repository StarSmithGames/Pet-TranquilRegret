using Game.Character;
using Game.Entities;
using Game.Systems.SheetSystem;
using Game.Systems.SheetSystem.Effects;

using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public class AffectionZone : InteractionZone
	{
		[SerializeField] private EffectData data;

		//private IEffect effect;

		//private EffectConverter effectConverter;

		//[Inject]
		//private void Construct(EffectConverter effectConverter)
		//{
		//	this.effectConverter = effectConverter;
		//}

		//protected override void OnEnter(Collider other)
		//{
		//	if (data == null) return;

		//	var character = other.GetComponentInParent<ICharacterModel>();
		//	if(character != null)
		//	{
		//		if(effect == null)
		//		{
		//			effect = effectConverter.Convert(data);
		//		}

		//		character.Sheet.Effects.Registrate(effect);
		//	}

		//	base.OnEnter(other);
		//}

		//protected override void OnExit(Collider other)
		//{
		//	if (data == null) return;

		//	var character = other.GetComponentInParent<ICharacterModel>();
		//	if (character != null)
		//	{
		//		character.Sheet.Effects.UnRegistrate(effect);
		//	}

		//	base.OnExit(other);
		//}
	}
}