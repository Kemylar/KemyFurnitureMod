using UnityEngine;
using KemyFurniture.Core;

namespace KemyFurniture.Items.SeaChest
{
    [RequireComponent(typeof(ShipItemCrate))]
    public class SeaChestLogic : MonoBehaviour, ICustomFurnitureLogic
    {
        public bool OverrideLookUI => true;
        public string CustomControlPrompt => "Open Chest";
        public bool HasCustomGrid => true;
        public Vector2 GridDimensions => new Vector2(4f, 4f);

        private CrateInventory inventory;

        private void Awake()
        {
            inventory = GetComponent<CrateInventory>();
        }

        public bool OnAltActivate(ShipItem item)
        {
            if (inventory != null)
            {
                inventory.OpenCrate();
                return false;
            }
            return true;
        }
    }
}