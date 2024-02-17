using UnityEngine;

using Zenject;

namespace Game.Systems.UISystem
{
	[CreateAssetMenu(fileName = "UIInstaller", menuName = "Installers/UIInstaller")]
	public class UIInstaller : ScriptableObjectInstaller<UIInstaller>
	{
		public UISettings UISettings;

		public override void InstallBindings()
		{
			Container.BindInstance( UISettings );
		}
	}
}