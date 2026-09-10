using UnityEngine;
using KemyFurniture.Core;

namespace KemyFurniture.Items.CaptainsDesk
{
    [RequireComponent(typeof(ShipItemCrate))]
    public class CaptainsDeskLogic : MonoBehaviour, ICustomFurnitureLogic
    {
        public bool OverrideLookUI => true;
        public string CustomControlPrompt => "Open Desk";
        public bool HasCustomGrid => true;
        public Vector2 GridDimensions => new Vector2(4f, 3f); // 12 slots (4 wide, 3 tall)

        public bool OnAltActivate(ShipItem item)
        {
            var inventory = GetComponent<CrateInventory>();
            if (inventory != null)
            {
                inventory.OpenCrate();
                return false;
            }
            return true;
        }
    }
}