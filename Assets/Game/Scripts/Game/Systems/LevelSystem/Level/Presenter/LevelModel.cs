namespace Game.Systems.LevelSystem
{
	public class LevelModel
	{
		public bool UseLives => false;

		public int Number { get; }
		public string Name => "";
		public string Type => "regular";

		public string GameMode => "regular";
		
		public LevelConfig Config { get; }

		public LevelModel(
			int number,
			LevelConfig config)
		{
			Number = number;
			Config = config;
		}
	}
}