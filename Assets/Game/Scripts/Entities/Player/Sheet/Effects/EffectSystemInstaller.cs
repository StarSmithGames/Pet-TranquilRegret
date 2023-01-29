using Game.Entities;

using Zenject;

namespace Game.Systems.SheetSystem.Effects
{
	public class EffectSystemInstaller : Installer<EffectSystemInstaller>
	{
		public override void InstallBindings()
		{
			//Container.BindFactory<InstantEffectData, ISheet, InstantEffect, InstantEffect.Factory>().NonLazy();
			//Container.BindFactory<PersistentEffectData, ISheet, ProcessEffect, ProcessEffect.Factory>().NonLazy();
			Container.BindFactory<InflictEffectData, ICharacter, InflictEffect, InflictEffect.Factory>().NonLazy();

			Container
				.BindFactory<EffectData, ICharacter, IEffect, EffectFactory>()
				.FromFactory<CustomEffectFactory>()
				.NonLazy();
		}
	}
}