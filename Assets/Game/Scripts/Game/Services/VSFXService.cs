using Game.Systems.GameSystem;
using Game.Systems.StorageSystem;

using StarSmithGames.Core;
using StarSmithGames.Go.AudioService;
using StarSmithGames.Go.VibrationService;

using Zenject;

namespace Game.Services
{
    public class VSFXService
    {
        [ Inject ] private GameplayConfig _gameplayConfig;
        [Inject] private IAudioService audioService;
        [Inject] private IVibrationService vibrationService;

        public void PlayUIButton()
        {
            audioService.PlaySound(_gameplayConfig.taps.RandomItem());
            vibrationService.Vibrate();
        }
    }
}