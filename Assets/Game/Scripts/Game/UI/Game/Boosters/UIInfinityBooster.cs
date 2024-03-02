using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public abstract class UIInfinityBooster : UIBooster
	{
		[ Space ]
		public TMPro.TextMeshProUGUI ProgressText;
		public Image Progress;
		public GameObject Bar;

		public void EnableBar( bool trigger )
		{
			Bar.SetActive( trigger );
		}
		
		public void SetFillAmountText( string value )
		{
			ProgressText.text = value;
		}
		
		public void SetFillAmount( float value )
		{
			Progress.fillAmount = value;
		}
	}
}