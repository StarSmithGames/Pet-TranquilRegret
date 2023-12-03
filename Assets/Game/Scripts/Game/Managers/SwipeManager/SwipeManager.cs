using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Game.Managers.SwipeManager
{
	public class SwipeManager : ITickable
	{
		public bool IsSwiping => swipeDirection != Swipe.None;
		public bool IsSwipingRight => IsSwipingDirection(Swipe.Right);
		public bool IsSwipingLeft => IsSwipingDirection(Swipe.Left);
		public bool IsSwipingUp => IsSwipingDirection(Swipe.Up);
		public bool IsSwipingDown => IsSwipingDirection(Swipe.Down);
		public bool IsSwipingDownLeft => IsSwipingDirection(Swipe.DownLeft);
		public bool IsSwipingDownRight => IsSwipingDirection(Swipe.DownRight);
		public bool IsSwipingUpLeft => IsSwipingDirection(Swipe.UpLeft);
		public bool IsSwipingUpRight => IsSwipingDirection(Swipe.UpRight);

		public delegate void OnSwipeDetectedHandler(Swipe swipeDirection, Vector2 swipeVelocity);
		public event OnSwipeDetectedHandler OnSwipeDetected
		{
			add
			{
				onSwipeDetected += value;
				autoDetectSwipes = true;
			}
			remove
			{
				onSwipeDetected -= value;
			}
		}
		private OnSwipeDetectedHandler onSwipeDetected;

		public const float eightDirAngle = 0.906f;
		public const float fourDirAngle = 0.5f;
		public const float defaultDPI = 72f;
		public const float dpcmFactor = 2.54f;

		private Dictionary<Swipe, Vector2> cardinalDirections = new Dictionary<Swipe, Vector2>()
	{
		{ Swipe.Up,         CardinalDirection.Up        },
		{ Swipe.Down,       CardinalDirection.Down      },
		{ Swipe.Right,      CardinalDirection.Right     },
		{ Swipe.Left,       CardinalDirection.Left      },
		{ Swipe.UpRight,    CardinalDirection.UpRight   },
		{ Swipe.UpLeft,     CardinalDirection.UpLeft    },
		{ Swipe.DownRight,  CardinalDirection.DownRight },
		{ Swipe.DownLeft,   CardinalDirection.DownLeft  }
	};

		public Vector2 SwipeVelocity { get; private set; }

		private float dpcm;
		private float swipeStartTime;
		private float swipeEndTime;
		private bool autoDetectSwipes;
		private bool swipeEnded;
		private Swipe swipeDirection;
		private Vector2 firstPressPos;
		private Vector2 secondPressPos;

		private Settings settings;

		public SwipeManager(Settings settings)
		{
			this.settings = settings;

			float dpi = (Screen.dpi == 0) ? defaultDPI : Screen.dpi;
			dpcm = dpi / dpcmFactor;
		}

		public void Tick()
		{
			if (autoDetectSwipes)
			{
				DetectSwipe();
			}
		}

		/// <summary>
		/// Attempts to detect the current swipe direction.
		/// Should be called over multiple frames in an Update-like loop.
		/// </summary>
		private void DetectSwipe()
		{
			if (GetTouchInput() || GetMouseInput())
			{
				// Swipe already ended, don't detect until a new swipe has begun
				if (swipeEnded)
				{
					return;
				}

				Vector2 currentSwipe = secondPressPos - firstPressPos;
				float swipeCm = currentSwipe.magnitude / dpcm;

				// Check the swipe is long enough to count as a swipe (not a touch, etc)
				if (swipeCm < settings.minSwipeLength)
				{
					// Swipe was not long enough, abort
					if (!settings.triggerSwipeAtMinLength)
					{
						if (Application.isEditor)
						{
							Debug.Log("[SwipeManager] Swipe was not long enough.");
						}

						swipeDirection = Swipe.None;
					}

					return;
				}

				swipeEndTime = Time.time;
				SwipeVelocity = currentSwipe * (swipeEndTime - swipeStartTime);
				swipeDirection = GetSwipeDirByTouch(currentSwipe);
				swipeEnded = true;

				if (onSwipeDetected != null)
				{
					onSwipeDetected(swipeDirection, SwipeVelocity);
				}
			}
			else
			{
				swipeDirection = Swipe.None;
			}
		}

		#region Helper Functions
		private bool GetTouchInput()
		{
			if (Input.touches.Length > 0)
			{
				Touch t = Input.GetTouch(0);

				// Swipe/Touch started
				if (t.phase == TouchPhase.Began)
				{
					firstPressPos = t.position;
					swipeStartTime = Time.time;
					swipeEnded = false;
					// Swipe/Touch ended
				}
				else if (t.phase == TouchPhase.Ended)
				{
					secondPressPos = t.position;
					return true;
					// Still swiping/touching
				}
				else
				{
					// Could count as a swipe if length is long enough
					if (settings.triggerSwipeAtMinLength)
					{
						return true;
					}
				}
			}

			return false;
		}

		private bool GetMouseInput()
		{
			// Swipe/Click started
			if (Input.GetMouseButtonDown(0))
			{
				firstPressPos = (Vector2)Input.mousePosition;
				swipeStartTime = Time.time;
				swipeEnded = false;
				// Swipe/Click ended
			}
			else if (Input.GetMouseButtonUp(0))
			{
				secondPressPos = (Vector2)Input.mousePosition;
				return true;
				// Still swiping/clicking
			}
			else
			{
				// Could count as a swipe if length is long enough
				if (settings.triggerSwipeAtMinLength)
				{
					return true;
				}
			}

			return false;
		}

		private bool IsDirection(Vector2 direction, Vector2 cardinalDirection)
		{
			var angle = settings.useEightDirections ? eightDirAngle : fourDirAngle;
			return Vector2.Dot(direction, cardinalDirection) > angle;
		}

		private Swipe GetSwipeDirByTouch(Vector2 currentSwipe)
		{
			currentSwipe.Normalize();
			var swipeDir = cardinalDirections.FirstOrDefault(dir => IsDirection(currentSwipe, dir.Value));
			return swipeDir.Key;
		}

		private bool IsSwipingDirection(Swipe swipeDir)
		{
			DetectSwipe();
			return swipeDirection == swipeDir;
		}
		#endregion

		[System.Serializable]
		public class Settings
		{
			[Tooltip("Min swipe distance (inches) to register as swipe")]
			public float minSwipeLength = 0.5f;

			[Tooltip("If true, a swipe is counted when the min swipe length is reached. If false, a swipe is counted when the touch/click ends.")]
			public bool triggerSwipeAtMinLength = false;

			[Tooltip("Whether to detect eight or four cardinal directions")]
			public bool useEightDirections = false;
		}
	}

	class CardinalDirection
	{
		public static readonly Vector2 Up = new Vector2(0, 1);
		public static readonly Vector2 Down = new Vector2(0, -1);
		public static readonly Vector2 Right = new Vector2(1, 0);
		public static readonly Vector2 Left = new Vector2(-1, 0);
		public static readonly Vector2 UpRight = new Vector2(1, 1);
		public static readonly Vector2 UpLeft = new Vector2(-1, 1);
		public static readonly Vector2 DownRight = new Vector2(1, -1);
		public static readonly Vector2 DownLeft = new Vector2(-1, -1);
	}

	public enum Swipe
	{
		None,
		Up,
		Down,
		Left,
		Right,
		UpLeft,
		UpRight,
		DownLeft,
		DownRight
	};
}