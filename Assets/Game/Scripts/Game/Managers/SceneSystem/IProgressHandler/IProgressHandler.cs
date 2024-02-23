namespace Game.Systems.SceneSystem
{
    public interface IProgressHandler
    {
        bool IsDone { get; }
        
        float GetProgress();
    }
}