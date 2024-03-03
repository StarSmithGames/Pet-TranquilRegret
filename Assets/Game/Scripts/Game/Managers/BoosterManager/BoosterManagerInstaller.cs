using Game.Systems.BoosterManager.Settings;
using UnityEngine;
using Zenject;

namespace Game.Systems.BoosterManager
{
	[CreateAssetMenu( fileName = "BoosterManagerInstaller", menuName = "Installers/BoosterManagerInstaller" )]
	public sealed class BoosterManagerInstaller : ScriptableObjectInstaller< BoosterManagerInstaller >
	{
		public SpeedUpBoosterSettings SpeedUpBoosterSettings;
		public VisionBoosterSettings VisionBoosterSettings;
		
		public override void InstallBindings()
		{
			Container.BindInstance( SpeedUpBoosterSettings );
			Container.BindInstance( VisionBoosterSettings );
			Container.BindInterfacesAndSelfTo< VisionBooster >().AsSingle().WhenInjectedInto< BoosterManager >();
			Container.BindInterfacesAndSelfTo< BoosterManager >().AsSingle();
		}
	}
}