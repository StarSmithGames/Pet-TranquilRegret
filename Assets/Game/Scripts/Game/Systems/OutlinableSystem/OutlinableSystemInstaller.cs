using EPOOutline;
using UnityEngine;
using Zenject;

namespace Game.Services
{
	[CreateAssetMenu( fileName = "OutlinableSystemInstaller", menuName = "Installers/OutlinableSystemInstaller" )]
	public sealed class OutlinableSystemInstaller : ScriptableObjectInstaller< OutlinableSystemInstaller >
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo< OutlinableManager >().AsSingle();
			Container.BindInterfacesAndSelfTo< OutlinableService >().AsSingle();
			Container.BindInterfacesAndSelfTo< OutlinableTracker >().AsSingle().NonLazy();
		}
	}
}