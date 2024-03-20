using Game.Entity.CharacterSystem;
using Game.Environment.PickableSystem;
using Game.Managers.CharacterManager;
using Game.Systems.UISystem;
using Game.UI;
using System;
using System.Collections.Generic;
using UnityEditor.Localization.Platform.Android;
using UnityEngine.EventSystems;
using Zenject;

namespace Game.VVM
{
	public sealed class ControlCanvasViewModel : MultipleViewModel< UIControlCanvas >
	{
		private Character _player;
		
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

			_characterManager.OnPlayerRegistrated += PlayerRegistratedHandler;
			
			ModelView.DropButton.gameObject.SetActive( false );
			ModelView.AttackButton.gameObject.SetActive( true );

			ModelView.OnDropButtonClicked += DropButtonClickedHandler;
			ModelView.OnAttackButtonClicked += AttackButtonClickedHandler;
			ModelView.Joystick.OnTapped += JumpButtonClickedHandler;
		}

		public override void Dispose()
		{
			_characterManager.OnPlayerRegistrated -= PlayerRegistratedHandler;
			if ( _player != null )
			{
				_player.Presenter.PickupObserver.OnPickupped -= PlayerPickuppedObject;
			}
			
			ModelView.OnDropButtonClicked -= DropButtonClickedHandler;
			ModelView.OnAttackButtonClicked -= AttackButtonClickedHandler;
			ModelView.Joystick.OnTapped -= JumpButtonClickedHandler;
			
			base.Dispose();
		}

		private void DropButtonClickedHandler()
		{
			_player.Presenter.PickupObserver.Drop();
			ModelView.DropButton.gameObject.SetActive( _player.Presenter.PickupObserver.IsHasDrop() );
		}
		
		private void AttackButtonClickedHandler()
		{
			_player.Presenter.Combat.Attack();
		}

		private void JumpButtonClickedHandler( PointerEventData eventData )
		{
			_player.Presenter.Controller.Jump();
		}

		private void PlayerRegistratedHandler()
		{
			_characterManager.OnPlayerRegistrated -= PlayerRegistratedHandler;

			_player = _characterManager.Player;
			_player.Presenter.PickupObserver.OnPickupped += PlayerPickuppedObject;
		}

		private void PlayerPickuppedObject( PickableObject pickableObject )
		{
			ModelView.DropButton.gameObject.SetActive( true );
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