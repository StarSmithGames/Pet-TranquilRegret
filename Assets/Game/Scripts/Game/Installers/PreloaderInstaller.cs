using Game.Systems.GameSystem;
using Game.Systems.UISystem;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
	public sealed class PreloaderInstaller : MonoInstaller<PreloaderInstaller>
	{
		[ Header("UI") ]
		public UIRootPreloader UIRootPreloader;

		public override void InstallBindings()
		{
			Container.Bind< UIRootPreloader >().FromComponentsInNewPrefab( UIRootPreloader ).AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo< Preloader >().AsSingle().NonLazy();
		}
	}
}