using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Systems.NavigationSystem
{
	public abstract class TapPointer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public event Action< PointerEventData > OnTapped;
		
		private float _tapTime = 0.16f;
		private int _tapCount;
		private float _timeSincePointerDown;
		private float _timeSincePointerUp;
		
		public virtual void OnPointerDown( PointerEventData eventData )
		{
			_timeSincePointerDown = Time.unscaledTime - eventData.clickTime;
		}

		public virtual void OnPointerUp( PointerEventData eventData )
		{
			_timeSincePointerUp = Time.unscaledTime - eventData.clickTime;
			
			if ( _timeSincePointerUp < _tapTime )
			{
				Tapped( eventData );
			}
		}

		protected virtual void Tapped( PointerEventData eventData )
		{
			OnTapped?.Invoke( eventData );
		}
	}
}