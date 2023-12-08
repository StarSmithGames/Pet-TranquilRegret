using Game.Services;

using StarSmithGames.Go;

using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace Game.UI
{
	[CreateAssetMenu(fileName = "UIInstaller", menuName = "Installers/UIInstaller")]
	public class UIInstaller : ScriptableObjectInstaller<UIInstaller>
	{
		public UIAward awardPrefab;

		public List<ViewBase> dialogs = new();

		public override void InstallBindings()
		{
			Container.BindFactory<UIAward, UIAward.Factory>().FromComponentInNewPrefab(awardPrefab).AsSingle();

			Container.BindInstance(dialogs);
			Container.BindInterfacesAndSelfTo<ViewService>().AsSingle().NonLazy();
		}
	}
}