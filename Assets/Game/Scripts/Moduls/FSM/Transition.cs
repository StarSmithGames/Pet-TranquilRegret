using System;

namespace Company.Module.FSM
{
	public sealed class Transition
	{
		private readonly Decision _decision;
		private readonly State _trueState;
		private readonly State _falseState;

		public Transition(
			Decision decision,
			State trueState,
			State falseState
			)
		{
			_decision = decision ?? throw new ArgumentNullException( nameof(decision) );
			_trueState = trueState ?? throw new ArgumentNullException( nameof(trueState) );
			_falseState = falseState ?? throw new ArgumentNullException( nameof(falseState) );
		}
		
		public void Execute( StateMachine stateMachine )
		{
			if ( _decision.Decide( stateMachine ) )
			{
				stateMachine.ChangedState( _trueState );
			}
			else
			{
				stateMachine.ChangedState( _falseState );
			}
		}
	}
}