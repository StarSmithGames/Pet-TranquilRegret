using Game.VFX;
using UnityEngine;
using Zenject;

namespace Game.Entities
{
	public class PlayerVFX : MonoBehaviour
	{
		[field: SerializeField] public Transform ContentUnder { get; private set; }
		[field: SerializeField] public ParticalVFX DustTrailEffect { get; private set; }

		private ParticalVFXPoofEffect.Factory poofEffectFactory;
		private ParticalVFXPoofEffect.Factory smallPoofEffectFactory;

		[Inject]
		public void Construct(
			[Inject(Id = "Poof")] ParticalVFXPoofEffect.Factory poofEffectFactory,
			[Inject(Id = "SmallPoof")] ParticalVFXPoofEffect.Factory smallPoofEffectFactory)
		{
			this.poofEffectFactory = poofEffectFactory;
			this.smallPoofEffectFactory = smallPoofEffectFactory;
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
	}
}