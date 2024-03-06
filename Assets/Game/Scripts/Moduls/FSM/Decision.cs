namespace Company.Module.FSM
{
	public abstract class Decision
	{
		public abstract bool Decide( StateMachine stateMachine );
	}
}