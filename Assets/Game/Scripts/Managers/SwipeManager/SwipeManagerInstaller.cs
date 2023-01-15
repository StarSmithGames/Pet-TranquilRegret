using UnityEngine;

using Zenject;

namespace Game.Managers.SwipeManager
{
    [CreateAssetMenu(fileName = "SwipeManagerInstaller", menuName = "Installers/SwipeManagerInstaller")]
    public class SwipeManagerInstaller : ScriptableObjectInstaller<SwipeManagerInstaller>
    {
		public SwipeManager.Settings settings;

		public override void InstallBindings()
		{
			Container.BindInstance(settings).WhenInjectedInto<SwipeManager>();
			Container.BindInterfacesAndSelfTo<SwipeManager>().AsSingle().NonLazy();
		}
	}
}