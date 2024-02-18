using UnityEngine;
using Zenject;

namespace Game.Systems.UISystem
{
	public sealed class UIRootPreloader : UIRoot
	{
		public ViewAggregator DialogAggregator { get; private set; }
		
		public Transform DialogRoot;
		
		[ Inject ]
		private void Construct(
			DiContainer container,
			UISettings uiSettings
		)
		{
			ViewCreator dialogCreator = new( container, DialogRoot );
			DialogAggregator = new( dialogCreator, uiSettings.PreloadDialogs );
		}
	}
}