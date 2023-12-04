using StarSmithGames.Core;

namespace Game.Systems.GoalSystem
{
	public interface IGoal : IValue<float>, IReadOnlyValue<float>, IObservableValue, IBar
	{
		GoalItemModel Model { get; }
		bool IsCompleted { get; }
	}
}