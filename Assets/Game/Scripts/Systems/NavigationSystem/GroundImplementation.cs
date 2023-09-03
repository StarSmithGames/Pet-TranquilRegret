using Game.Character;

using System;

using UnityEngine;

using Zenject;

namespace Game.Systems.NavigationSystem
{
	//public abstract class GroundImplementation : ITickable
	//{
	//	protected ICharacterModel character;
	//	//protected CharacterController controller;

	//	public GroundImplementation(ICharacterModel character, TickableManager tickableManager)
	//	{
	//		this.character = character;

	//		tickableManager.Add(this);
	//	}

	//	public abstract void Tick();
	//}

	//public class PlayerGroundImplementation : GroundImplementation
	//{
	//	private Player player;

	//	public PlayerGroundImplementation(Player player, TickableManager tickableManager) : base(player, tickableManager)
	//	{
	//		this.player = player;

	//		//controller.onJumped += OnJumped;
	//		//controller.onLanded += OnLanded;
	//	}

	//	public override void Tick()
	//	{
	//		//CheckGround(controller.CurrentGroundLayer);
	//	}

	//	private void Disable()
	//	{
	//		//player.PlayerVFX.DustTrailEffect.Enable(false);
	//	}

	//	private void CheckGround(GroundLayer groundLayer)
	//	{
	//		if (groundLayer == null)
	//		{
	//			Disable();

	//			return;
	//		}

	//		//if (controller.IsGrounded && !controller.IsJumping)
	//		//{
	//		//	if (controller.IsIdling())
	//		//	{
	//		//		Disable();
	//		//	}
	//		//	else if (controller.IsMoving() && controller.MovingMagnitude > 0.1f)
	//		//	{
	//		//		if (groundLayer is GroundLayerEarth)
	//		//		{
	//		//			player.PlayerVFX.DustTrailEffect.Enable(true);
	//		//		}
	//		//		else
	//		//		{
	//		//			Disable();
	//		//		}
	//		//	}

	//		//	player.PlayerVFX.PawStep();
	//		//}
	//		//else
	//		//{
	//		//	Disable();
	//		//}
	//	}

	//	private void OnJumped()
	//	{
	//	}

	//	private void OnLanded()
	//	{
	//		//if (controller.CurrentGroundLayer is GroundLayerSoft) return;

	//		//if(controller.CurrentGroundLayer is GroundLayerEarth)
	//		//{
	//		//	player.PlayerVFX.Poof();
	//		//}
	//		//else
	//		//{
	//		//	player.PlayerVFX.SmallPoof();
	//		//}

	//		//player.PlayerVFX.PawSteps();
	//	}

	//	public class Factory : PlaceholderFactory<Player, PlayerGroundImplementation> { }
	//}

	//public class GroudImplementationFactory : PlaceholderFactory<ICharacterModel, GroundImplementation> { }

	//public class CustomGroudImplementationFactory : IFactory<ICharacterModel, GroundImplementation>
	//{
	//	private PlayerGroundImplementation.Factory playerGroundFactory;

	//	public CustomGroudImplementationFactory(PlayerGroundImplementation.Factory playerGroundFactory)
	//	{
	//		this.playerGroundFactory = playerGroundFactory;
	//	}

	//	public GroundImplementation Create(ICharacterModel character)
	//	{
	//		if (character is Player player)
	//		{
	//			return playerGroundFactory.Create(player);
	//		}

	//		throw new NotImplementedException();
	//	}
	//}
}