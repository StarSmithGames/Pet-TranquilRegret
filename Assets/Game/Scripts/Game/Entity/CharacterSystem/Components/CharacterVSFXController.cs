using Game.VFX;
using UnityEngine;
using Zenject;

namespace Game.Entity.CharacterSystem
{
	public class CharacterVSFXController : MonoBehaviour
	{
		[field: SerializeField] public Transform ContentUnder { get; private set; }
		[field: SerializeField] public ParticalVFX DustTrailEffect { get; private set; }
		
		[SerializeField] private float stepDelta = 0.7f;
		[SerializeField] private float stepGap = 0.5f;

		private Vector3 lastStepEmit;
		private int stepDir = 1;

		[Inject] private CharacterPresenter presenter;

		private CharacterView View => presenter.View;

		private ParticalVFXPoofEffect.Factory poofEffectFactory;
		private ParticalVFXPoofEffect.Factory smallPoofEffectFactory;
		private ParticalVFXFootStep.Factory footStepFactory;
		private ParticalVFXFootStep.Factory pawStepFactory;

		[Inject]
		public void Construct(
			[Inject(Id = "Poof")] ParticalVFXPoofEffect.Factory poofEffectFactory,
			[Inject(Id = "SmallPoof")] ParticalVFXPoofEffect.Factory smallPoofEffectFactory,
			[Inject(Id = "StepFootPrint")] ParticalVFXFootStep.Factory footStepFactory,
			[Inject(Id = "StepPawHorizontalPrint")] ParticalVFXFootStep.Factory pawStepFactory)
		{
			this.poofEffectFactory = poofEffectFactory;
			this.smallPoofEffectFactory = smallPoofEffectFactory;
			this.footStepFactory = footStepFactory;
			this.pawStepFactory = pawStepFactory;
		}

		private void Start()
		{
			lastStepEmit = View.root.position;
		}

		public void FootStep()
		{
			Step(footStepFactory);
		}

		public void FootSteps()
		{
			Steps(footStepFactory);
		}

		public void PawStep()
		{
			Step(pawStepFactory);
		}

		public void PawSteps()
		{
			Steps(pawStepFactory);
		}

		public void Poof()
		{
			var poof = poofEffectFactory.Create();
			poof.transform.SetParent(ContentUnder);
			poof.transform.localPosition = Vector3.zero;
			poof.Play();
		}

		public void SmallPoof()
		{
			var poof = smallPoofEffectFactory.Create();
			poof.transform.SetParent(ContentUnder);
			poof.transform.localPosition = Vector3.zero;
			poof.Play();
		}


		private void Step(ParticalVFXFootStep.Factory factory)
		{
			if (Vector3.Distance(lastStepEmit, View.root.position) > stepDelta)
			{
				var step = factory.Create();
				stepDir *= -1;

				step.Play(new ParticleSystem.EmitParams()
				{
					position = View.root.position + (View.model.right * stepGap * stepDir),
					rotation = View.model.rotation.eulerAngles.y,
				});

				lastStepEmit = View.root.position;
			}
		}

		private void Steps(ParticalVFXFootStep.Factory factory)
		{
			var stepLeft = factory.Create();
			var stepRight = factory.Create();

			stepLeft.Play(new ParticleSystem.EmitParams()
			{
				position = View.root.position + (View.model.right * stepGap),
				rotation = View.model.rotation.eulerAngles.y,
			});

			stepRight.Play(new ParticleSystem.EmitParams()
			{
				position = View.root.position + (View.model.right * stepGap * -1),
				rotation = View.model.rotation.eulerAngles.y,
			});

			lastStepEmit = View.root.position;
		}
	}
}