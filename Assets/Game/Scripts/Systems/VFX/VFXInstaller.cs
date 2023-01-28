using UnityEngine;
using Zenject;

namespace Game.VFX
{
	[CreateAssetMenu(fileName = "VFXInstaller", menuName = "Installers/VFXInstaller")]
    public class VFXInstaller : ScriptableObjectInstaller<VFXInstaller>
    {
		public ParticalVFXFootStep stepPawHorizontalPrintEffect;
		public ParticalVFXFootStep stepPawVerticalPrintEffect;

		public override void InstallBindings()
		{
			Container
				.BindFactory<ParticalVFXFootStep, ParticalVFXFootStep.Factory>().WithId("StepPawHorizontalPrint")
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(10)
				.FromComponentInNewPrefab(stepPawHorizontalPrintEffect));

			Container
				.BindFactory<ParticalVFXFootStep, ParticalVFXFootStep.Factory>().WithId("StepPawVerticalPrint")
				.FromMonoPoolableMemoryPool((x) => x.WithInitialSize(10)
				.FromComponentInNewPrefab(stepPawVerticalPrintEffect));
		}
	}
}