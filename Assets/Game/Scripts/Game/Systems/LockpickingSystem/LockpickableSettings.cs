namespace Game.Systems.LockpickingSystem
{
	[System.Serializable]
	public class LockpickableSettings
	{
		public bool isLocked = true;
		public float unlockTime = 2.5f;
		public float decreaseSpeed = 1f;
	}
}