using UnityEngine;
using Zenject;

namespace Game.VFX
{
	[CreateAssetMenu(fileName = "VFXInstaller", menuName = "Installers/VFXInstaller")]
    public class VFXInstaller : ScriptableObjectInstaller<VFXInstaller>
    {
		public ParticalVFXPoofEffect poofEffect;
		public ParticalVFXPoofEffect smallPoofEffect;

		public override void InstallBindings()
		{
			Container
				.BindFactory<ParticalVFXPoofEffect, ParticalVFXPoofEffect.Factory>().WithId("Poof")
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(1)
				.FromComponentInNewPrefab(poofEffect));

			Container
				.BindFactory<ParticalVFXPoofEffect, ParticalVFXPoofEffect.Factory>().WithId("SmallPoof")
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(1)
				.FromComponentInNewPrefab(smallPoofEffect));
		}
	}
}