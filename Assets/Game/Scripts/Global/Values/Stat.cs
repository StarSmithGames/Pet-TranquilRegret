using Game.UI;

public abstract class Stat : AttributeModifiableFloat
{
	protected Stat(float value) : base(value) { }
}

public class MoveSpeed : Stat
{
	public MoveSpeed(float value) : base(value) { }
}

public class JumpImpulse : Stat
{
	public JumpImpulse(float value) : base(value) { }
}

public class ThrowImpulse : Stat, IEnableable
{
	public bool IsEnable { get; private set; }

	public override float TotalValue => IsEnable ? base.TotalValue : 0;

	public ThrowImpulse(float value) : base(value) { }


	public void Enable(bool trigger)
	{
		IsEnable = trigger;
	}
}