using StarSmithGames.Go;

using UnityEngine;

namespace Game.UI
{
	public abstract class UICanvas : MonoBehaviour
	{
		public ViewRegistrator ViewRegistrator
		{
			get
			{
				if (viewRegistrator == null)
				{
					viewRegistrator = new ViewRegistrator();
				}

				return viewRegistrator;
			}
		}
		protected ViewRegistrator viewRegistrator;

		public Transform Windows
		{
			get
			{
				if (windows == null)
				{
					windows = transform.Find("Windows");
				}

				return windows;
			}
		}
		[SerializeField] protected Transform windows;

		public Transform VFX
		{
			get
			{
				if (vfx == null)
				{
					vfx = transform.Find("VFX");
				}

				return vfx;
			}
		}
		[SerializeField] protected Transform vfx;
	}
}