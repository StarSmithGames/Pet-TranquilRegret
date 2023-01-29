using Game.Systems.SheetSystem;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

namespace Game.Entities
{
	public class PlayerInstaller : MonoInstaller<PlayerInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<EffectConverter>().AsSingle();
		}
	}
}