using UnityEngine;
using KemyFurniture.Core;

namespace KemyFurniture.Items.Cabinet
{
    [RequireComponent(typeof(ShipItemCrate))]
    public class CabinetSmallLogic : MonoBehaviour, ICustomFurnitureLogic
    {
        public bool OverrideLookUI => true;
        public string CustomControlPrompt => "Open Nightstand";
        public bool HasCustomGrid => true;
        public Vector2 GridDimensions => new Vector2(4f, 3f); // 12 slots

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