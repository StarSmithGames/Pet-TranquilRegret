using Game.Entities;

namespace Game.Systems.SheetSystem.Effects
{
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

	public class AddStatModifierEnchantment : Enchantment
	{
		private AttributeModifier modifier;

		private Stat stat;

		public AddStatModifierEnchantment(Stat stat, float add)
		{
			this.stat = stat;

			modifier = new AddModifier(add);
		}

		public override void Activate()
		{
			base.Activate();
			stat.AddModifier(modifier);
		}

		public override void Deactivate()
		{
			base.Deactivate();
			stat.RemoveModifier(modifier);
		}
	}


	[System.Serializable]
	public abstract class EnchantmentType
	{
		public abstract Enchantment GetEnchantment(object obj);
	}

	[System.Serializable]
	public class AddMoveSpeed : EnchantmentType
	{
		public float add;

		public override Enchantment GetEnchantment(object obj)
		{
			var sheet = (obj as ICharacter).Sheet;

			return new AddStatModifierEnchantment(sheet.MoveSpeed, add);
		}
	}

	[System.Serializable]
	public class AddJumpImpulse : EnchantmentType
	{
		public float add;

		public override Enchantment GetEnchantment(object obj)
		{
			var sheet = (obj as ICharacter).Sheet;

			return new AddStatModifierEnchantment(sheet.JumpImpulse, add);
		}
	}

	[System.Serializable]
	public class AddThrowImpulse : EnchantmentType
	{
		public float add;

		public override Enchantment GetEnchantment(object obj)
		{
			var sheet = (obj as ICharacter).Sheet;

			return new AddStatModifierEnchantment(sheet.ThrowImpulse, add);
		}
	}
}