using UnityEngine;

using Zenject;

namespace Game.Systems.LevelSystem
{
	public class LevelSystemInstaller : Installer<LevelSystemInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();
		}
	}
}