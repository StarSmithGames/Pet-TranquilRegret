using Game.Environment.SurfaceSystem;
using Game.Systems.NavigationSystem;

using Zenject;

namespace Game.Entity.CharacterSystem
{
	public class CharacterGroundImplementation : ITickable
	{
		private CharacterController _controller;
		private CharacterVSFXController _vsfxController;

		public CharacterGroundImplementation(
			CharacterController controller,
			CharacterVSFXController vsfxController,
			TickableManager tickableManager)
		{
			_controller = controller;
			_vsfxController = vsfxController;

			controller.onJumped += OnJumped;
			controller.onLanded += OnLanded;

			tickableManager.Add(this);
		}

		public void Tick()
		{
			CheckGround(_controller.CurrentSurfaceLayer);
		}

		private void EnableDustTrail(bool trigger)
		{
			_vsfxController.DustTrailEffect.Enable(trigger);
		}

		private void DoPawSteps(bool isOne)
		{
			if (isOne)
			{
				_vsfxController.PawStep();
			}
			else
			{
				_vsfxController.PawSteps();
			}
		}

		private void CheckGround(SurfaceLayer surfaceLayer)
		{
			if (surfaceLayer == null)
			{
				EnableDustTrail(false);

				return;
			}

			if (_controller.IsGrounded && !_controller.IsJumping)
			{
				if (_controller.IsIdling())
				{
					EnableDustTrail(false);
				}
				else if (_controller.IsMoving() && _controller.MovingMagnitude > 0.1f)
				{
					if (surfaceLayer is SurfaceLayerEarth)
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
			if (_controller.CurrentSurfaceLayer is SurfaceLayerSoft) return;

			if (_controller.CurrentSurfaceLayer is SurfaceLayerEarth)
			{
				_vsfxController.Poof();
			}
			else
			{
				_vsfxController.SmallPoof();
			}

			DoPawSteps(false);
		}
	}
}