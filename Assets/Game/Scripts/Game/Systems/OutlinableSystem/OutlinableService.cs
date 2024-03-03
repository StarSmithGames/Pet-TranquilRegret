using Cysharp.Threading.Tasks;
using DG.Tweening;
using EPOOutline;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game.Services
{
	public sealed class OutlinableService
	{
		private readonly OutlinableManager _outlinableManager;
		
		public OutlinableService(
			OutlinableManager outlinableManager
			)
		{
			_outlinableManager = outlinableManager ?? throw new ArgumentNullException( nameof(outlinableManager) );
		}
		
		public void Enable( bool trigger )
		{
			for ( int i = 0; i < _outlinableManager.Observers.Count; i++ )
			{
				_outlinableManager.Observers[ i ].enabled = trigger;
			}
		}

		public void SetOutlineData( OutlineData data )
		{
			for ( int i = 0; i < _outlinableManager.Observers.Count; i++ )
			{
				_outlinableManager.Observers[ i ].SetData( data );
			}
		}

		public void Fade( float value )
		{
			try
			{
				for ( int i = 0; i < _outlinableManager.Observers.Count; i++ )
				{
					var item = _outlinableManager.Observers[ i ];
					Color color = item.OutlineParameters.Color;
					color.a = value;
					item.OutlineParameters.Color = color;
					
					Color colorFill = item.OutlineParameters.FillPass.GetColor( SerializedPassPropertyIds.BASIC_COLOR_FILL );
					colorFill.a = value;
					item.OutlineParameters.FillPass.SetColor( SerializedPassPropertyIds.BASIC_COLOR_FILL, colorFill );
				}
			}
			catch ( Exception e )
			{
				Debug.LogError( e );
			}
		}
		
		public UniTask DOFadeColorFill( float endValue, float duraion, Ease ease, CancellationToken token = default )
		{
			var taskList = new List<UniTask>();
			
			for ( int i = 0; i < _outlinableManager.Observers.Count; i++ )
			{
				taskList.Add( 
					_outlinableManager.Observers[ i ].OutlineParameters.DOFade( endValue, duraion )
						.SetEase( ease )
						.ToUniTask( cancellationToken: token ));
				
				taskList.Add( 
					_outlinableManager.Observers[ i ].OutlineParameters.FillPass.DOFade( SerializedPassPropertyIds.BASIC_COLOR_FILL, endValue, duraion )
						.SetEase( ease )
						.ToUniTask( cancellationToken: token ));
			}

			return UniTask.WhenAll( taskList );
		}
	}
}