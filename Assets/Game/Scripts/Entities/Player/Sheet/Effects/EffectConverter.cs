using Game.Entities;
using Game.Systems.SheetSystem.Effects;
using System;

using Zenject;

namespace Game.Systems.SheetSystem
{
	public class EffectConverter
	{
		private EffectFactory effectFactory;

		public EffectConverter(EffectFactory effectFactory)
		{
			this.effectFactory = effectFactory;
		}

		public IEffect Convert(EffectData data)
		{
			return effectFactory.Create(data);
		}
	}

	public sealed class EffectFactory : PlaceholderFactory<EffectData, IEffect> { }

	public sealed class CustomEffectFactory : IFactory<EffectData, IEffect>
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

		public IEffect Create(EffectData data)
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
				return infictFactory.Create(inflictData);
			}

			throw new NotImplementedException();
		}
	}
}