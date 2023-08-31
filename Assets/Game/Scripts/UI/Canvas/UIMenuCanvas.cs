using Game.HUD.Menu;

using UnityEngine;

namespace Game.UI
{
    public class UIMenuCanvas : UICanvas
    {
        [field: SerializeField] public UIRoadPin Pin { get; private set; }
    }
}