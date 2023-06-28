using StarSmithGames.Go;

namespace Game.Services
{
	public class ViewService
	{
		public ViewRegistrator DialogViewRegistrator
		{
			get
			{
				if(dialogViewRegistrator == null)
				{
					dialogViewRegistrator = new ViewRegistrator();
				}

				return dialogViewRegistrator;
			}
		}
		private ViewRegistrator dialogViewRegistrator;
	}
}