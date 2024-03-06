using Game.Managers.CharacterManager;
using Game.Systems.UISystem;
using Game.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Zenject;

namespace Game.VVM
{
	public sealed class ControlCanvasViewModel : MultipleViewModel< UIControlCanvas >
	{
		private readonly UIRootGame _rootGame;
		private readonly CharacterManager _characterManager;
		
		public ControlCanvasViewModel( 
			DiContainer diContainer,
			
			UIRootGame rootGame,
			CharacterManager characterManager
			) : base( diContainer )
		{
			_rootGame = rootGame ?? throw new ArgumentNullException( nameof(rootGame) );
			_characterManager = characterManager ?? throw new ArgumentNullException( nameof(characterManager) );
		}

		public override void Initialize()
		{
			EnableView( true );
			
			ModelView.DropButton.gameObject.SetActive( false );
			ModelView.AttackButton.gameObject.SetActive( true );

			ModelView.OnDropButtonClicked += DropButtonClickedHandler;
			ModelView.OnAttackButtonClicked += AttackButtonClickedHandler;
			ModelView.Joystick.OnTapped += JumpButtonClickedHandler;
		}

		public override void Dispose()
		{
			ModelView.OnDropButtonClicked -= DropButtonClickedHandler;
			ModelView.OnAttackButtonClicked -= AttackButtonClickedHandler;
			ModelView.Joystick.OnTapped -= JumpButtonClickedHandler;
			
			base.Dispose();
		}

		private void DropButtonClickedHandler()
		{
			
		}
		
		private void AttackButtonClickedHandler()
		{
			_characterManager.Player.Presenter.Combat.Attack();
		}

		private void JumpButtonClickedHandler( PointerEventData eventData )
		{
			_characterManager.Player.Presenter.Controller.Jump();
		}
		
		protected override UIControlCanvas GetView() => _rootGame.ControlCanvas;
		
		protected override List< Type > GetRuntimeViewModels()
		{
			return new()
			{
				typeof(SpeedUpBoosterViewModel),
				typeof(VisionBoosterViewModel),
			};
		}
	}
}