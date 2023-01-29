using Game.Entities;
using Game.Systems.SheetSystem.Effects;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Systems.SheetSystem
{
	public class EffectConverter
	{
		private ICharacter character;
		private EffectFactory effectFactory;

		public EffectConverter(ICharacter character, EffectFactory effectFactory)
		{
			this.character = character;
			this.effectFactory = effectFactory;
		}

		public IEffect Convert(EffectData data)
		{
			return effectFactory.Create(data, character);
		}
	}

	public sealed class EffectFactory : PlaceholderFactory<EffectData, ICharacter, IEffect> { }

	public sealed class CustomEffectFactory : IFactory<EffectData, ICharacter, IEffect>
	{
		//private InstantEffect.Factory instantFactory;
		//private ProcessEffect.Factory processFactory;
		private InflictEffect.Factory infictFactory;

		public CustomEffectFactory(InflictEffect.Factory infictFactory)
		{
			//this.instantFactory = instantFactory;
			//this.processFactory = processFactory;
			this.infictFactory = infictFactory;
		}

		public IEffect Create(EffectData data, ICharacter character)
		{
			//if (data is InstantEffectData instantData)
			//{
			//	return instantFactory.Create(instantData, sheet);
			//}
			//else if (data is PersistentEffectData processData)
			//{
			//	return processFactory.Create(processData, sheet);
			//}
			if (data is InflictEffectData inflictData)
			{
				return infictFactory.Create(inflictData, character);
			}

			throw new NotImplementedException();
		}
	}
}