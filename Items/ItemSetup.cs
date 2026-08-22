using System;
using UnityEngine;
using KemyFurniture.Items.Cabinet;
using KemyFurniture.Items.Carpet;
using KemyFurniture.Items.ScrollShelf;
using KemyFurniture.Items.SeaChest;

namespace KemyFurniture.Core
{
    public static class ItemSetup
    {
        public static void RegisterSaveIndex(GameObject prefab, int saveIndex)
        {
            if (prefab == null) return;

            var saveable = prefab.GetComponent<SaveablePrefab>() ?? prefab.AddComponent<SaveablePrefab>();
            saveable.prefabIndex = saveIndex;
        }

        public static void ConfigurePrefabProperties(GameObject prefab)
        {
            if (prefab == null) return;

            string name = prefab.name.ToLower();
            var shipItem = prefab.GetComponent<ShipItem>();

            // 1. Small Cabinet (Nightstand)
            if (name.Contains("cabinetsmall"))
            {
                EnsureCrateComponents(prefab);
                if (shipItem != null)
                {
                    shipItem.name = "CabinetSmall";
                    shipItem.lookText = "Nightstand";
                }
                if (prefab.GetComponent<CabinetSmallLogic>() == null)
                {
                    prefab.AddComponent<CabinetSmallLogic>();
                }
                SetItemValue(prefab, 480);
            }
            // 2. Wide Cabinet (Dresser)
            else if (name.Contains("cabinetwide"))
            {
                EnsureCrateComponents(prefab);
                if (shipItem != null)
                {
                    shipItem.name = "CabinetWide";
                    shipItem.lookText = "Dresser";
                }
                if (prefab.GetComponent<CabinetWideLogic>() == null)
                {
                    prefab.AddComponent<CabinetWideLogic>();
                }
                SetItemValue(prefab, 720);
            }
            // 3. Tall Cabinet (Original)
            else if (name.Contains("cabinet"))
            {
                EnsureCrateComponents(prefab);
                if (shipItem != null)
                {
                    shipItem.name = "Cabinet";
                    shipItem.lookText = "Cabinet";
                }
                if (prefab.GetComponent<CabinetLogic>() == null)
                {
                    prefab.AddComponent<CabinetLogic>();
                }
                SetItemValue(prefab, 1200);
            }
            // 4. Sea Chest
            else if (name.Contains("chest") || name.Contains("seachest"))
            {
                EnsureCrateComponents(prefab);
                if (shipItem != null)
                {
                    shipItem.name = "SeaChest";
                    shipItem.lookText = "Sea Chest";
                }
                if (prefab.GetComponent<SeaChestLogic>() == null)
                {
                    prefab.AddComponent<SeaChestLogic>();
                }
                SetItemValue(prefab, 800);
            }
            // 5. Scroll Shelf
            else if (name.Contains("scroll") || name.Contains("shelf"))
            {
                EnsureCrateComponents(prefab);
                if (shipItem != null)
                {
                    shipItem.name = "ScrollShelf";
                    shipItem.lookText = "Scroll Shelf";
                }
                if (prefab.GetComponent<ScrollShelfLogic>() == null)
                {
                    prefab.AddComponent<ScrollShelfLogic>();
                }
                SetItemValue(prefab, 450);
            }
            // 6. Carpets
            else if (name.Contains("carpet"))
            {
                if (prefab.GetComponent<CarpetLogic>() == null)
                {
                    prefab.AddComponent<CarpetLogic>();
                }
                SetItemValue(prefab, 400);
            }
            // 7. Navigator's Drafting Table
            else if (name.Contains("navigatortable") || name.Contains("table"))
            {
                SetItemValue(prefab, 650);
            }
            // 8. Bunk Bed
            else if (name.Contains("bed"))
            {
                SetItemValue(prefab, 950);
            }
        }

        private static void EnsureCrateComponents(GameObject prefab)
        {
            if (prefab.GetComponent<CrateInventory>() == null)
            {
                prefab.AddComponent<CrateInventory>();
            }

            var btn = prefab.GetComponent<GoPointerButton>() ?? prefab.AddComponent<GoPointerButton>();
            btn.enabled = true;
        }

        private static void SetItemValue(GameObject prefab, int value)
        {
            var shipItem = prefab.GetComponent<ShipItem>();
            if (shipItem != null)
            {
                shipItem.value = value;
            }
        }
    }
}