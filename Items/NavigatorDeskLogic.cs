using UnityEngine;
using KemyFurniture.Core;

namespace KemyFurniture.Items.NavigatorDesk
{
    public class NavigatorDeskLogic : MonoBehaviour, ICustomFurnitureLogic
    {
        // The desk uses vanilla look text ("pick up") and has no container backend
        public bool OverrideLookUI => false;
        public string CustomControlPrompt => string.Empty;

        public bool HasCustomGrid => false;
        public Vector2 GridDimensions => Vector2.zero;

        public bool OnAltActivate(ShipItem item)
        {
            return true; // Let vanilla execution pass through
        }
    }
}