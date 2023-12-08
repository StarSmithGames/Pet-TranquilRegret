using Game.Systems.PreferencesSystem;

using StarSmithGames.Core;
using StarSmithGames.Core.StorageSystem;

namespace Game.Systems.StorageSystem
{
	public sealed class GameFastData
	{
		public bool IsGDPRApplied
		{
			get => InputOutput.PlayerPrefsGet("is_gdpr_applied").CastObject<bool>(false);
			set => InputOutput.PlayerPrefsSet("is_gdpr_applied", value);
		}

		public bool IsFirstTime
		{
			get => InputOutput.PlayerPrefsGet("is_first_time").CastObject<bool>(true);
			set => InputOutput.PlayerPrefsSet("is_first_time", value);
		}

		public int TimeInGame
		{
			get => InputOutput.PlayerPrefsGet("time_in_game").CastObject<int>(0);
			set => InputOutput.PlayerPrefsSet("time_in_game", value);
		}

		public int SessionsCount
		{
			get => InputOutput.PlayerPrefsGet("sessions_count").CastObject<int>(0);
			set => InputOutput.PlayerPrefsSet("sessions_count", value);
		}

		public int LanguageIndex
		{
			get => InputOutput.PlayerPrefsGet("language").CastObject<int>(0);
			set => InputOutput.PlayerPrefsSet("language", value);
		}

		public int LastRegularLevelIndex
		{
			get => InputOutput.PlayerPrefsGet("last_regular_level").CastObject<int>(0);
			set => InputOutput.PlayerPrefsSet("last_regular_level", value);
		}

		public FastData<int> SoftCoins { get; private set; } = new("soft_coins", 0);

		public FastData<int> HardDiamonds { get; private set; } = new("hard_diamonds", 0);


		public FastData<PreferencesData> PreferencesParams { get; private set; } = new("player_preferences");

		public FastData<CloseData> CloseParams { get; private set; } = new("close_params");
	}
}