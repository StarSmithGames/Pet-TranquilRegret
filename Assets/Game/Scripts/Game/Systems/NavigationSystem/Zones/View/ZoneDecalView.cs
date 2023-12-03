using Game.VFX;

namespace Game.Systems.ZoneSystem
{
	public class ZoneDecalView : ZoneView
	{
		public DecalVFX decal;

		protected override void Awake()
		{
			base.Awake();

			DoIdle();
		}

		private void DoIdle()
		{
			decal?.DoIdle();
		}

		private void DoEnter()
		{
			decal?.DoKill();
			decal?.ScaleTo(1.2f);
		}

		private void ResetAnimation()
		{
			decal?.ScaleTo(1f, callback: () => decal.DoIdle());
		}

		protected override void OnZoneCollectionChanged()
		{
			if(zone.Registrator.registers.Count > 0)
			{
				DoEnter();
			}
			else
			{
				ResetAnimation();
			}
		}
	}
}