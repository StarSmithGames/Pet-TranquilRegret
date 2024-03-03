using StarSmithGames.Core;

namespace Game.Systems.SheetSystem.Effects
{
	[System.Serializable]
	public abstract class Enchantment : IActivation
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
	
	
	// [System.Serializable]
	// public class AddJumpImpulse : EnchantmentType
	// {
	// 	public float add;
	//
	// 	public override Enchantment GetEnchantment(object obj)
	// 	{
	// 		var sheet = (obj as ICharacterModel).Sheet;
	//
	// 		return new AddStatModifierEnchantment(sheet.JumpImpulse, add);
	// 	}
	// }
	//
	// [System.Serializable]
	// public class AddThrowImpulse : EnchantmentType
	// {
	// 	public float add;
	//
	// 	public override Enchantment GetEnchantment(object obj)
	// 	{
	// 		var sheet = (obj as ICharacterModel).Sheet;
	//
	// 		return new AddStatModifierEnchantment(sheet.ThrowImpulse, add);
	// 	}
	// }
}