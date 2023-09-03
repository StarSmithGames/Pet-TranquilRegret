using StarSmithGames.Go.SceneManager;

namespace Game.Extensions
{
	public static class SceneManagerExtensions
	{
		public static bool IsMenu(this SceneManager sceneManager)
		{
			return sceneManager.GetActiveScene().buildIndex == 1;
		}

		public static bool IsLevel(this SceneManager sceneManager)
		{
			return sceneManager.GetActiveScene().buildIndex > 1;
		}
	}
}