namespace Company.Module.FSM
{
	public abstract class StateMachine
	{
		protected State _currentState;
        
		public virtual void Tick()
		{
			_currentState?.Tick();
		}

		public virtual void FixedTick()
		{
			_currentState?.FixedTick();
		}
        
		public void ChangedState( State state )
		{
			if( _currentState == state) return;
            
			_currentState?.Exit();
			_currentState = state;
			_currentState?.Enter();
		}
	}
}