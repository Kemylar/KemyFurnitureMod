using UnityEngine;
using KemyFurniture.Core;

namespace KemyFurniture.Items.Cabinet
{
    [RequireComponent(typeof(ShipItemCrate))]
    public class CabinetWideLogic : MonoBehaviour, ICustomFurnitureLogic
    {
        public bool OverrideLookUI => true;
        public string CustomControlPrompt => "Open Dresser";
        public bool HasCustomGrid => true;
        public Vector2 GridDimensions => new Vector2(6f, 3f); // 18 slots

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