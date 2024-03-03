using StarSmithGames.Core;

namespace Game.Systems.SheetSystem.Effects
{
	public abstract class EnchantmentType : IActivation
	{
		public bool IsActive { get; private set; }

		public virtual void Activate()
		{
			IsActive = true;
		}

		public virtual void Deactivate()
		{
			IsActive = false;
		}
	}
}