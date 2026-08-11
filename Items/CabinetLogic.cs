using UnityEngine;
using KemyFurniture.Core;

namespace KemyFurniture.Items.Cabinet
{
    [RequireComponent(typeof(ShipItemCrate))]
    public class CabinetLogic : MonoBehaviour, ICustomFurnitureLogic
    {
        public bool OverrideLookUI => true;
        public string CustomControlPrompt => "Open Cabinet";
        public bool HasCustomGrid => true;

        // 30 total inventory slots (5 Columns x 6 Rows)
        public Vector2 GridDimensions => new Vector2(5f, 6f);

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