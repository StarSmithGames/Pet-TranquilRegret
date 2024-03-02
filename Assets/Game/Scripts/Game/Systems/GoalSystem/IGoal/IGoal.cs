using StarSmithGames.Core;

namespace Game.Systems.GoalSystem
{
	public interface IGoal : IValue<float>, IBar
	{
		GoalItemModel Model { get; }
		bool IsCompleted { get; }
	}
}