using Game.Managers.CharacterManager;
using Game.Services;
using System;
using UnityEngine;
using Zenject;

namespace Game.UI
{
	public sealed class UIGameScreen : MonoBehaviour
	{
		private CharacterManager _characterManager;
		
		[ Inject ]
		private void Construct(
			CharacterManager characterManager
			)
		{
			_characterManager = characterManager ?? throw new ArgumentNullException( nameof(characterManager) );
		}
		
		public void OnSettingsButtonClick()
		{
			// _viewService.TryShowDialog< SettingsDialog >();
		}
		
		public void OnJumpButtonClick()
		{
			_characterManager.Player.Presenter.Controller.Jump();
		}
		
		public void OnDropButtonClick()
		{
			
		}
	}
}