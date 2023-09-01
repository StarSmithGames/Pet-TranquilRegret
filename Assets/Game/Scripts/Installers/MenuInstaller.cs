using Game.Managers.GameManager;
using Game.UI;

using UnityEngine;

using Zenject;

namespace Game.Installers
{
	public class MenuInstaller : MonoInstaller
    {
		public UIMenuCanvas menuCanvas;

		public UIGoalItem goalPrefab;

		public override void InstallBindings()
		{
			Container
				.BindFactory<UIGoalItem, UIGoalItem.Factory>()
				.FromComponentInNewPrefab(goalPrefab)
				.AsSingle();

			Container.Bind<UICanvas>().FromInstance(menuCanvas);

#if UNITY_EDITOR
			var gameManager = Container.Resolve<GameManager>();
			if (gameManager.CurrentGameState == GameState.Empty)
			{
				gameManager.ChangeState(GameState.Menu);
			}
#endif
		}
	}
}