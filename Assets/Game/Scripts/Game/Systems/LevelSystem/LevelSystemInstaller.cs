using UnityEngine;

using Zenject;

namespace Game.Systems.LevelSystem
{
	[CreateAssetMenu(fileName = "LevelSystemInstaller", menuName = "Installers/LevelSystemInstaller")]
	public class LevelSystemInstaller : ScriptableObjectInstaller<LevelSystemInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle().NonLazy();
		}
	}
}