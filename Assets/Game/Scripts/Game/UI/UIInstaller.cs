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
		public List<ViewBase> dialogs = new();

		public override void InstallBindings()
		{
			Container.BindInstance(dialogs);
			Container.BindInterfacesAndSelfTo<ViewService>().AsSingle().NonLazy();
		}
	}
}