using Zenject;

namespace Game.Services.TickableService
{
    public sealed class TickableServiceInstaller : Installer< TickableServiceInstaller >
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo< TickableService >().AsSingle().NonLazy();
        }
    }
}