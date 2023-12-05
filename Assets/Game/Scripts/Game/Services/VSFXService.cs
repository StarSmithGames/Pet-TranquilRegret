using Game.Systems.StorageSystem;

using StarSmithGames.Core;
using StarSmithGames.Go.AudioService;
using StarSmithGames.Go.VibrationService;

using Zenject;

namespace Game.Services
{
    public class VSFXService
    {
        [Inject] private StorageSystem gameData;
        [Inject] private IAudioService audioService;
        [Inject] private IVibrationService vibrationService;

        public void PlayUIButton()
        {
            audioService.PlaySound(gameData.IntermediateData.GameplayConfig.taps.RandomItem());
            vibrationService.Vibrate();
        }
    }
}