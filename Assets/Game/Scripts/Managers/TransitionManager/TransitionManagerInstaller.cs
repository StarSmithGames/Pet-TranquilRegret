using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.Managers.TransitionManager
{
	[CreateAssetMenu(fileName = "TransitionManagerInstaller", menuName = "Installers/TransitionManagerInstaller")]
	public class TransitionManagerInstaller : ScriptableObjectInstaller<TransitionManagerInstaller>
	{
		public UIIntermediateCanvas subCanvasPrefab;

		public InfinityLoadingSettings infinityLoadingSettings;

		public override void InstallBindings()
		{
			Container.BindInstance(Container.InstantiatePrefabForComponent<UIIntermediateCanvas>(subCanvasPrefab));


			Container.BindInstance(infinityLoadingSettings).WhenInjectedInto<InfinityLoading>();
			Container.BindInterfacesAndSelfTo<InfinityLoading>().AsSingle();
			Container.BindInterfacesAndSelfTo<TransitionManager>().AsSingle();
		}
	}
}