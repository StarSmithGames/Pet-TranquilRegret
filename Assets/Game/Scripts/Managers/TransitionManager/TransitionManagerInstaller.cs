using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.Managers.TransitionManager
{
	[CreateAssetMenu(fileName = "TransitionManagerInstaller", menuName = "Installers/TransitionManagerInstaller")]
	public class TransitionManagerInstaller : ScriptableObjectInstaller<TransitionManagerInstaller>
	{
		public InfinityLoading infinityLoadingPrefab;

		public InfinityLoadingSettings infinityLoadingSettings;

		public override void InstallBindings()
		{
			Container.BindInstance(infinityLoadingSettings).WhenInjectedInto<InfinityLoading>();

			//Container
			//	.Bind<InfinityLoading>()
			//	.FromComponentInNewPrefab(infinityLoadingPrefab).AsSingle().NonLazy();
			//Container.BindInterfacesAndSelfTo<TransitionManager>().AsSingle();
		}
	}
}