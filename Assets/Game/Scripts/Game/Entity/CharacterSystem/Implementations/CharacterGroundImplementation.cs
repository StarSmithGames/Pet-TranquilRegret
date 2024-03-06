using Game.Systems.NavigationSystem;

using Zenject;

namespace Game.Entity.CharacterSystem
{
	public class CharacterGroundImplementation : ITickable
	{
		private CharacterController controller;
		private CharacterVSFXController vsfxController;

		public CharacterGroundImplementation(
			CharacterController controller,
			CharacterVSFXController vsfxController,
			TickableManager tickableManager)
		{
			this.controller = controller;
			this.vsfxController = vsfxController;

			controller.onJumped += OnJumped;
			controller.onLanded += OnLanded;

			tickableManager.Add(this);
		}

		public void Tick()
		{
			CheckGround(controller.CurrentGroundLayer);
		}

		private void EnableDustTrail(bool trigger)
		{
			vsfxController.DustTrailEffect.Enable(trigger);
		}

		private void DoPawSteps(bool isOne)
		{
			if (isOne)
			{
				vsfxController.PawStep();
			}
			else
			{
				vsfxController.PawSteps();
			}
		}

		private void CheckGround(GroundLayer groundLayer)
		{
			if (groundLayer == null)
			{
				EnableDustTrail(false);

				return;
			}

			if (controller.IsGrounded && !controller.IsJumping)
			{
				if (controller.IsIdling())
				{
					EnableDustTrail(false);
				}
				else if (controller.IsMoving() && controller.MovingMagnitude > 0.1f)
				{
					if (groundLayer is GroundLayerEarth)
					{
						EnableDustTrail(true);
					}
					else
					{
						EnableDustTrail(false);
					}
				}

				DoPawSteps(true);
			}
			else
			{
				EnableDustTrail(false);
			}
		}

		private void OnJumped()
		{
		}

		private void OnLanded()
		{
			if (controller.CurrentGroundLayer is GroundLayerSoft) return;

			if (controller.CurrentGroundLayer is GroundLayerEarth)
			{
				vsfxController.Poof();
			}
			else
			{
				vsfxController.SmallPoof();
			}

			DoPawSteps(false);
		}
	}
}