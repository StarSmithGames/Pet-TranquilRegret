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
		private ViewService _viewService;
		
		[ Inject ]
		private void Construct(
			CharacterManager characterManager,
			ViewService viewService
			)
		{
			_characterManager = characterManager ?? throw new ArgumentNullException( nameof(characterManager) );
			_viewService = viewService ?? throw new ArgumentNullException( nameof(viewService) );
		}
		
		public void OnSettingsButtonClick()
		{
			_viewService.TryShowDialog< SettingsDialog >();
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