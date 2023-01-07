using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.SettingsSystem
{
	public class UILanguagePanel : MonoBehaviour
	{
		[field: SerializeField] public TMPro.TextMeshProUGUI Language { get; private set; }
		[field: SerializeField] public Image Flag { get; private set; }
		[field: SerializeField] public TMPro.TextMeshProUGUI Country { get; private set; }
		[field: SerializeField] public Button Left { get; private set; }
		[field: SerializeField] public Button Right { get; private set; }
	}
}