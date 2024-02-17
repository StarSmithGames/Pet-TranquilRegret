using Game.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Systems.InfinityRoadSystem
{
#if UNITY_EDITOR
	[ExecuteInEditMode]
#endif
	public class UIRoadLevel : MonoBehaviour
	{
		public event UnityAction< UIRoadLevel > OnButtonClicked;

		public bool IsEnable { get; private set; } = true;

		public Canvas Canvas;
		public TMPro.TextMeshProUGUI Text;
		public Image On;
		public Image Off;
		public Button Button;
		public List< UILevelStar > Stars = new();
		
#if UNITY_EDITOR
		private void Update()
		{
			if ( Application.isPlaying ) return;

			Text.text = gameObject.name.Split("_")[1];
		}
#endif

		public void Enable(bool trigger)
		{
			IsEnable = trigger;

			On.enabled = IsEnable;
			Off.enabled = !IsEnable;
		}

		/// <param name="count">[0-3]</param>
		public void EnableStars( int count )
		{
			for ( int i = 0; i < Stars.Count; i++ )
			{
				Stars[ i ].Activate( i <= count - 1 );
			}
		}

		public void SetCamera( Camera camera )
		{
			Canvas.worldCamera = camera;
		}
		
		public void OnButtonClick()
		{
			OnButtonClicked?.Invoke( this );
		}
	}
}