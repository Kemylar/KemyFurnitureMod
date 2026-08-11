using UnityEngine;
using KemyFurniture.Core;

namespace KemyFurniture.Items.Bed
{
    public class BedLogic : MonoBehaviour, ICustomFurnitureLogic
    {
        // Intercept Look UI to prompt sleeping
        public bool OverrideLookUI => true;
        public string CustomControlPrompt => "Sleep";

        // Bed does not use container grids
        public bool HasCustomGrid => false;
        public Vector2 GridDimensions => Vector2.zero;

        public bool OnAltActivate(ShipItem item)
        {
            // Return true to allow native ShipItemBed.OnAltActivate() execution
            return true;
        }
    }
}