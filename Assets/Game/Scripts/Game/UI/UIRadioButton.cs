using DG.Tweening;
using Sirenix.OdinInspector;
using StarSmithGames.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public class UIRadioButton : MonoBehaviour
	{
		public Image on;
		public Image off;
		[ Header("Can Be NULL") ]
		public Image ToggleOn;
		public Image ToggleOff;
		public RectTransform Handle;

		private float _handleLeft = -55f;
		private float _handleRight = 55f;
		
		private Sequence _sequence;
		
		[Button(DirtyOnClick = true)]
		public void Enable(bool enable)
		{
			on.SetAlpha( enable ? 1f : 0f );
			off.SetAlpha( enable ? 0f : 1f );
			
			if ( ToggleOn == null || ToggleOff == null || Handle == null ) return;
			ToggleOn.SetAlpha( enable ? 1f : 0f );
			ToggleOff.SetAlpha( enable ? 0f : 1f );
			Handle.DOLocalMoveX( enable ? _handleRight : _handleLeft, 0f );
		}

		public void DoAnimation( bool trigger )
		{
			if ( trigger )
			{
				DoOn();
			}
			else
			{
				DoOff();
			}
		}
		
		public void DoOn()
		{
			_sequence?.Kill( true );
			_sequence = DOTween.Sequence()
				.Append( on.DOFade( 1f, 0.33f ) )
				.Join( off.DOFade( 0f, 0.33f ) );
				
			if ( ToggleOn == null || ToggleOff == null || Handle == null ) return;

			_sequence.Join( ToggleOn.DOFade( 1f, 0.33f ) )
				.Join( ToggleOff.DOFade( 0f, 0.33f ) )
				.Join( Handle.DOLocalMoveX( _handleRight, 0.33f, true ) );
		}

		public void DoOff()
		{
			_sequence?.Kill( true );
			_sequence = DOTween.Sequence()
				.Append( on.DOFade( 0f, 0.33f ) )
				.Join( off.DOFade( 1f, 0.33f ) );
			
			if ( ToggleOn == null || ToggleOff == null || Handle == null ) return;
			_sequence.Join( ToggleOn.DOFade( 0f, 0.33f ) )
				.Join( ToggleOff.DOFade( 1f, 0.33f ) )
				.Join( Handle.DOLocalMoveX( _handleLeft, 0.33f, true ) );
		}
	}
}