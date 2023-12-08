using Zenject;

namespace Game.Managers.RewardManager
{
	public class RewardManagerInstaller : Installer<RewardManagerInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<RewardManager>().AsSingle();
		}
	}
}