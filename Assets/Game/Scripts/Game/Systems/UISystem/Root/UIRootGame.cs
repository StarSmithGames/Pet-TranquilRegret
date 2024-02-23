using Game.UI;
using System.Linq;
using Zenject;

namespace Game.Systems.UISystem
{
	public class UIRootGame : UIRoot
	{
		public ViewAggregator DialogAggregator { get; private set; }
		
		public UIGameCanvas GameCanvas;
		public UIDynamicCanvas DynamicCanvas;
		public UIFrontCanvas FrontCanvas;
		
		[ Inject ]
		private void Construct(
			DiContainer container,
			UISettings uiSettings
		)
		{
			ViewCreator dialogCreator = new( container, DynamicCanvas.DialogsRoot );
			DialogAggregator = new( dialogCreator, uiSettings.GameDialogs.Union( uiSettings.CommonDialogs ).ToList()  );
		}
	}
}