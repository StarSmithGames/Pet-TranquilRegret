using UnityEditor;

namespace Game.Systems.SpawnSystem
{
	public static class SpawnPointEditor
	{
		[MenuItem("GameObject/Spawn/Point", false, 0)]
		public static void Create(MenuCommand command)
		{
			CreateUtility.CreateObject("SpawnPoint", typeof(SpawnPoint));
		}
	}
}