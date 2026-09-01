using System;
using UnityEngine;
using KemyFurniture.Items.Bed;
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

            // 1. Small Cabinet (Nightstand & Al'Ankh Variant)
            if (name.Contains("cabinetsmall"))
            {
                EnsureCrateComponents(prefab);
                if (shipItem != null)
                {
                    shipItem.name = name.Contains("alankh") ? "CabinetSmallAlAnkh" : "CabinetSmall";
                    shipItem.lookText = name.Contains("alankh") ? "Al'Ankh Nightstand" : "Nightstand";
                }
                if (prefab.GetComponent<CabinetSmallLogic>() == null)
                {
                    prefab.AddComponent<CabinetSmallLogic>();
                }
                SetItemValue(prefab, 480);
            }
            // 2. Wide Cabinet (Dresser & Al'Ankh Variant)
            else if (name.Contains("cabinetwide"))
            {
                EnsureCrateComponents(prefab);
                if (shipItem != null)
                {
                    shipItem.name = name.Contains("alankh") ? "CabinetWideAlAnkh" : "CabinetWide";
                    shipItem.lookText = name.Contains("alankh") ? "Al'Ankh Dresser" : "Dresser";
                }
                if (prefab.GetComponent<CabinetWideLogic>() == null)
                {
                    prefab.AddComponent<CabinetWideLogic>();
                }
                SetItemValue(prefab, 720);
            }
            // 3. Tall Cabinet (Original & Al'Ankh Variant)
            else if (name.Contains("cabinet"))
            {
                EnsureCrateComponents(prefab);
                if (shipItem != null)
                {
                    shipItem.name = name.Contains("alankh") ? "CabinetAlAnkh" : "Cabinet";
                    shipItem.lookText = name.Contains("alankh") ? "Al'Ankh Cabinet" : "Cabinet";
                }
                if (prefab.GetComponent<CabinetLogic>() == null)
                {
                    prefab.AddComponent<CabinetLogic>();
                }
                SetItemValue(prefab, 1200);
            }
            // 4. Sea Chest & Al'Ankh Chest
            else if (name.Contains("chest") || name.Contains("seachest"))
            {
                EnsureCrateComponents(prefab);
                if (shipItem != null)
                {
                    shipItem.name = name.Contains("alankh") ? "ChestAlAnkh" : "SeaChest";
                    shipItem.lookText = name.Contains("alankh") ? "Al'Ankh Chest" : "Sea Chest";
                }
                if (prefab.GetComponent<SeaChestLogic>() == null)
                {
                    prefab.AddComponent<SeaChestLogic>();
                }
                SetItemValue(prefab, 800);
            }
            // 5. Scroll Shelf & Al'Ankh Variant
            else if (name.Contains("scroll") || name.Contains("shelf"))
            {
                EnsureCrateComponents(prefab);
                if (shipItem != null)
                {
                    shipItem.name = name.Contains("alankh") ? "ScrollShelfAlAnkh" : "ScrollShelf";
                    shipItem.lookText = name.Contains("alankh") ? "Al'Ankh Scroll Shelf" : "Scroll Shelf";
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
            // 8. Bunk Bed & Al'Ankh Variant
            else if (name.Contains("bed"))
            {
                if (shipItem != null)
                {
                    shipItem.name = name.Contains("alankh") ? "BedAlAnkh" : "Bed";
                    shipItem.lookText = name.Contains("alankh") ? "Al'Ankh Bed" : "Bed";
                }
                if (prefab.GetComponent<BedLogic>() == null)
                {
                    prefab.AddComponent<BedLogic>();
                }
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