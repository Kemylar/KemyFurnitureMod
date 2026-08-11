using UnityEngine;
using KemyFurniture.Core;

namespace KemyFurniture.Items.ScrollShelf
{
    [RequireComponent(typeof(ShipItemCrate))]
    public class ScrollShelfLogic : MonoBehaviour, ICustomFurnitureLogic
    {
        public bool OverrideLookUI => true;
        public string CustomControlPrompt => "Open Shelf";
        public bool HasCustomGrid => true;
        public Vector2 GridDimensions => new Vector2(3f, 4f);

        private CrateInventory inventory;
        private Transform[] visualScrolls;

        private void Awake()
        {
            inventory = GetComponent<CrateInventory>();

            // Find scroll nodes anywhere in the hierarchy
            visualScrolls = new Transform[12];
            Transform[] allTransforms = GetComponentsInChildren<Transform>(true);

            for (int i = 0; i < 12; i++)
            {
                string targetNameD2 = $"Scroll_{i:D2}";
                string targetNameSimple = $"Scroll_{i}";

                foreach (Transform t in allTransforms)
                {
                    if (t.name == targetNameD2 || t.name == targetNameSimple)
                    {
                        visualScrolls[i] = t;
                        break;
                    }
                }
            }
        }

        private void Start()
        {
            UpdateScrollVisibilities();
        }

        private void Update()
        {
            UpdateScrollVisibilities();
        }

        private void UpdateScrollVisibilities()
        {
            int count = (inventory != null && inventory.containedItems != null)
                ? inventory.containedItems.Count
                : 0;

            for (int i = 0; i < visualScrolls.Length; i++)
            {
                if (visualScrolls[i] != null)
                {
                    visualScrolls[i].gameObject.SetActive(i < count);
                }
            }
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