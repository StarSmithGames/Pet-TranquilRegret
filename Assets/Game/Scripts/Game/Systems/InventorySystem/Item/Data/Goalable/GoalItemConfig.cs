using Sirenix.OdinInspector;

namespace Game.Systems.InventorySystem
{
    public abstract class GoalItemConfig : ItemConfig
    {
        [HideLabel] public Information information;
    }
}