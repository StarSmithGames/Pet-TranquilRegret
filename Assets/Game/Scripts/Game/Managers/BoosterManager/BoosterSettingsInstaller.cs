using Game.Systems.BoosterManager.Settings;
using UnityEngine;
using Zenject;

namespace Game.Systems.BoosterManager
{
	[CreateAssetMenu( fileName = "BoosterSettingsInstaller", menuName = "Installers/BoosterSettingsInstaller" )]
	public sealed class BoosterSettingsInstaller : ScriptableObjectInstaller< BoosterSettingsInstaller >
	{
		public BoosterSettings Settings;
	
		public override void InstallBindings()
		{
			Container.BindInstance( Settings );
		}
	}
}