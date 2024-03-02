using Game.Systems.BoosterManager;
using Game.Systems.UISystem;
using Game.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.VVM
{
	public sealed class ControlCanvasViewModel : MultipleViewModel< UIControlCanvas >
	{
		private readonly UIRootGame _rootGame;
		
		public ControlCanvasViewModel( 
			DiContainer diContainer,
			
			UIRootGame rootGame
			) : base( diContainer )
		{
			_rootGame = rootGame ?? throw new ArgumentNullException( nameof(rootGame) );
		}

		public override void Initialize()
		{
			EnableView( true );
		}

		protected override UIControlCanvas GetView() => _rootGame.ControlCanvas;
		
		protected override List< Type > GetRuntimeViewModels()
		{
			return new()
			{
				typeof(VisionBoosterViewModel),
			};
		}
	}
}