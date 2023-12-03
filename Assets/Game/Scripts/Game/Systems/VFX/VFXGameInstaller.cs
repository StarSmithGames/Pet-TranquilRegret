using Game.UI;
using Game.VFX.Markers;
using UnityEngine;

using Zenject;

namespace Game.VFX
{
	public class VFXGameInstaller : MonoInstaller<VFXGameInstaller>
	{
		[Header("Particals")]
		public ParticalVFXFootStep stepFootPrintEffect;
		public ParticalVFXPoofEffect poofEffect;
		public ParticalVFXPoofEffect smallPoofEffect;
		//[Header("Markers")]
		//public UIPointer pointerUIPrefab;

		public override void InstallBindings()
		{
			//Particles
			Container
				.BindFactory<ParticalVFXFootStep, ParticalVFXFootStep.Factory>().WithId("StepFootPrint")
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(1)
				.FromComponentInNewPrefab(stepFootPrintEffect));

			Container
				.BindFactory<ParticalVFXPoofEffect, ParticalVFXPoofEffect.Factory>().WithId("Poof")
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(1)
				.FromComponentInNewPrefab(poofEffect));

			Container
				.BindFactory<ParticalVFXPoofEffect, ParticalVFXPoofEffect.Factory>().WithId("SmallPoof")
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(1)
				.FromComponentInNewPrefab(smallPoofEffect));


			//UI
			//Container
			//	.BindFactory<UIPointer, UIPointer.Factory>()
			//	.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(1)
			//	.FromComponentInNewPrefab(pointerUIPrefab)
			//	.UnderTransform((x) => x.Container.Resolve<UISubCanvas>().VFX));
		}
	}
}