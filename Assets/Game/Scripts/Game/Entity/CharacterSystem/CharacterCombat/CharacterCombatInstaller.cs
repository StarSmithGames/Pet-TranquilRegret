using Game.Systems.CombatSystem;
using Zenject;

namespace Game.Entity.CharacterSystem
{
	public sealed class CharacterCombatInstaller : Installer< CharacterCombatInstaller >
	{
		public override void InstallBindings()
		{
			Container.Bind< AttackAggregator >().AsSingle();
			Container.Bind< CharacterCombat >().AsSingle();
		}
	}
}