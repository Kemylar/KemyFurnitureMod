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

            // Find scroll nodes Scroll_01 through Scroll_12 anywhere in the hierarchy
            visualScrolls = new Transform[12];
            Transform[] allTransforms = GetComponentsInChildren<Transform>(true);

            for (int i = 0; i < 12; i++)
            {
                int scrollNum = i + 1; // 1 to 12
                string targetNameD2 = $"Scroll_{scrollNum:D2}"; // Scroll_01 ... Scroll_12
                string targetNameSimple = $"Scroll_{scrollNum}";  // Scroll_1 ... Scroll_12

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