using UnityEngine;

using Zenject;

namespace Game.Systems.FloatingSystem
{
	[CreateAssetMenu(fileName = "FloatingSystemInstaller", menuName = "Installers/FloatingSystemInstaller")]
	public class FloatingSystemInstaller : ScriptableObjectInstaller<FloatingSystemInstaller>
	{
		public Floating3DText floating3DTextPrefab;
		public Floating3DTextWithIcon floating3DTextWithIconPrefab;

		public override void InstallBindings()
		{
			Container
				.BindFactory<Floating3DText, Floating3DText.Factory>()
				.FromMonoPoolableMemoryPool((pool) => pool.WithInitialSize(1)
				.FromComponentInNewPrefab(floating3DTextPrefab))
				.WhenInjectedInto<FloatingSystem>();

			Container
				.BindFactory<Floating3DTextWithIcon, Floating3DTextWithIcon.Factory>()
				.FromMonoPoolableMemoryPool((pool) => pool.WithInitialSize(3)
				.FromComponentInNewPrefab(floating3DTextWithIconPrefab))
				.WhenInjectedInto<FloatingSystem>();

			Container.BindInterfacesAndSelfTo<FloatingSystem>().AsSingle().NonLazy();
		}
	}
}