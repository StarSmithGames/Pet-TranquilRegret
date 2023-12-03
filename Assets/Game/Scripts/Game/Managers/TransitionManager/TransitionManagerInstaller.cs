using UnityEngine;

using Zenject;

namespace Game.Managers.TransitionManager
{
	[CreateAssetMenu(fileName = "TransitionManagerInstaller", menuName = "Installers/TransitionManagerInstaller")]
	public class TransitionManagerInstaller : ScriptableObjectInstaller<TransitionManagerInstaller>
	{
		public InfinityLoading infinityLoadingPrefab;

		public override void InstallBindings()
		{
			Container
				.Bind<InfinityLoading>()
				.FromComponentInNewPrefab(infinityLoadingPrefab).AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<TransitionManager>().AsSingle();
		}
	}
}