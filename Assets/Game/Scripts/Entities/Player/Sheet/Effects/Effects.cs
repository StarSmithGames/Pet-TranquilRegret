using Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

using Zenject;

namespace Game.Systems.SheetSystem.Effects
{
	public class Effects : Registrator<IEffect>
	{
		private ICharacter character;
		private EffectFactory effectFactory;

		public Effects(ICharacter character)
		{
			this.character = character;
			this.effectFactory = effectFactory;
		}
	}

	#region Effects
	public interface IEffect : IActivation
	{
		public event UnityAction<IEffect> onActivationChanged;

		EffectData Data { get; }
	}

	public abstract class Effect : IEffect
	{
		public event UnityAction<IEffect> onActivationChanged;

		public bool IsActive { get; private set; }

		public virtual EffectData Data { get; private set; }

		public virtual void Activate()
		{
			IsActive = true;

			onActivationChanged?.Invoke(this);
		}

		public virtual void Deactivate()
		{
			IsActive = false;

			onActivationChanged?.Invoke(this);
		}
	}

	public class InflictEffect : Effect
	{
		private List<Enchantment> enchantments;

		public override EffectData Data => data;
		private InflictEffectData data;

		public InflictEffect(InflictEffectData data, ICharacter character)
		{
			this.data = data;

			enchantments = data.enchantments.Select((x) => x.GetEnchantment(character)).ToList();
		}

		public override void Activate()
		{
			base.Activate();

			for (int i = 0; i < enchantments.Count; i++)
			{
				enchantments[i].Activate();
			}
		}

		public override void Deactivate()
		{
			for (int i = 0; i < enchantments.Count; i++)
			{
				enchantments[i].Deactivate();
			}

			base.Deactivate();
		}

		public class Factory : PlaceholderFactory<InflictEffectData, ICharacter, InflictEffect> { }
	}
	#endregion

}