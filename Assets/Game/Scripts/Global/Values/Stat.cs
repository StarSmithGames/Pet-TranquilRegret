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

public class ThrowImpulse : Stat
{
	public ThrowImpulse(float value) : base(value) { }
}