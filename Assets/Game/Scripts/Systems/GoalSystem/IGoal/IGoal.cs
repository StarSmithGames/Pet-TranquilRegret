using StarSmithGames.Core;

namespace Game.Systems.GoalSystem
{
	public interface IGoal : IValue<float>, IReadOnlyValue<float>, IObservableValue, IBar
	{
		GoalConfigWrapper ConfigWrapper { get; }
		bool IsCompleted { get; }
	}
}