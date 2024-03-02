using Game.VVM;
using System;
using UnityEngine;

namespace Game.UI
{
	public abstract class UIBooster : View
	{
		public event Action OnButtonClicked;
		
		[ Space ]
		public TMPro.TextMeshProUGUI CountText;
		public GameObject Count;
		public GameObject ADS;

		public void EnableCount( bool trigger )
		{
			Count.SetActive( trigger );
			ADS.SetActive( !trigger );
		}

		public void DisableCount()
		{
			Count.SetActive( false );
			ADS.SetActive( false );
		}

		public void SetCount( string value )
		{
			CountText.text = value;
		}
		
		public void OnButtonClick()
		{
			OnButtonClicked?.Invoke();
		}
	}
}