using UnityEngine;
using KemyFurniture.Core;

namespace KemyFurniture.Items.Carpet
{
    public class CarpetLogic : MonoBehaviour, ICustomFurnitureLogic
    {
        // Simple item; uses default Look UI and has no container grid
        public bool OverrideLookUI => false;
        public string CustomControlPrompt => string.Empty;

        public bool HasCustomGrid => false;
        public Vector2 GridDimensions => Vector2.zero;

        public bool OnAltActivate(ShipItem item)
        {
            return true; // Pass through to vanilla behavior
        }
    }
}