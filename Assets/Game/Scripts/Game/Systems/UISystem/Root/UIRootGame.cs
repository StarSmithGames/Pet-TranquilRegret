using Game.UI;
using Game.VVM;
using Game.VVM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Game.Systems.UISystem
{
	public class UIRootGame : UIRoot
	{
		public ViewAggregator DialogAggregator { get; private set; }
		
		public UIControlCanvas ControlCanvas;
		public UIGameCanvas GameCanvas;
		public UIDynamicCanvas DynamicCanvas;
		public UIFrontCanvas FrontCanvas;

		private ViewModelService _viewModelService;
		
		private List< Type > _runtimeViewModels = new()
		{
			typeof(ControlCanvasViewModel),
		};
		
		[ Inject ]
		private void Construct(
			DiContainer container,
			UISettings uiSettings
		)
		{
			ViewCreator dialogCreator = new( container, DynamicCanvas.DialogsRoot );
			DialogAggregator = new( dialogCreator, uiSettings.GameDialogs.Union( uiSettings.CommonDialogs ).ToList() );

			_viewModelService = new( container );
			
			Initialize();
		}

		public void Initialize()
		{
			for ( int i = 0; i < _runtimeViewModels.Count; i++ )
			{
				_viewModelService.Create( _runtimeViewModels[ i ] );
			}
		}

		public void Dispose()
		{
			_viewModelService?.Dispose();
		}

		private void OnDestroy()
		{
			Dispose();
		}
	}
}