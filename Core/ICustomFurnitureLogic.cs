using UnityEngine;

namespace KemyFurniture.Core
{
    public interface ICustomFurnitureLogic
    {
        bool OverrideLookUI { get; }
        string CustomControlPrompt { get; }
        bool HasCustomGrid { get; }
        Vector2 GridDimensions { get; }
        bool OnAltActivate(ShipItem item);
    }
}