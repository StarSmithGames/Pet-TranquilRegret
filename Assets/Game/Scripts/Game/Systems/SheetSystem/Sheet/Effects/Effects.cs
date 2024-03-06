using Game.Entity.CharacterSystem;

using StarSmithGames.Core;

using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.SheetSystem.Effects
{
	//public class Effects : Registrator<IEffect>
	//{
	//	private ICharacterModel character;

	//	public Effects(ICharacterModel character)
	//	{
	//		this.character = character;
	//	}

	//	protected override void OnRegistrated(IEffect effect)
	//	{
	//		base.OnRegistrated(effect);
	//		effect.Activate(character);
	//	}

	//	protected override void OnUnRegistrated(IEffect effect)
	//	{
	//		base.OnUnRegistrated(effect);
	//		effect.Deactivate();
	//	}
	//}

	//#region Effects
	//public interface IEffect : IActivation<ICharacterModel>
	//{
	//	public event UnityAction<IEffect> onActivationChanged;

	//	EffectData Data { get; }
	//}

	//public abstract class Effect : IEffect
	//{
	//	public event UnityAction<IEffect> onActivationChanged;

	//	public bool IsActive { get; private set; }

	//	public virtual EffectData Data { get; private set; }

	//	public virtual void Activate(ICharacterModel character)
	//	{
	//		IsActive = true;

	//		onActivationChanged?.Invoke(this);
	//	}

	//	public virtual void Deactivate()
	//	{
	//		IsActive = false;

	//		onActivationChanged?.Invoke(this);
	//	}
	//}

	//public class InflictEffect : Effect
	//{
	//	private List<Enchantment> enchantments;

	//	public override EffectData Data => data;
	//	private InflictEffectData data;

	//	public InflictEffect(InflictEffectData data)
	//	{
	//		this.data = data;
	//	}

	//	public override void Activate(ICharacterModel character)
	//	{
	//		if(enchantments == null)
	//		{
	//			enchantments = data.enchantments.Select((x) => x.GetEnchantment(character)).ToList();
	//		}

	//		for (int i = 0; i < enchantments.Count; i++)
	//		{
	//			enchantments[i].Activate();
	//		}

	//		base.Activate(character);
	//	}

	//	public override void Deactivate()
	//	{
	//		for (int i = 0; i < enchantments.Count; i++)
	//		{
	//			enchantments[i].Deactivate();
	//		}

	//		base.Deactivate();
	//	}

	//	public class Factory : PlaceholderFactory<InflictEffectData, InflictEffect> { }
	//}
	//#endregion
}