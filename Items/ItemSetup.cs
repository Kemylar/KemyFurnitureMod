using System;
using UnityEngine;
using KemyFurniture.Items.Bed;
using KemyFurniture.Items.Cabinet;
using KemyFurniture.Items.CaptainsDesk;
using KemyFurniture.Items.Carpet;
using KemyFurniture.Items.NavigatorDesk;
using KemyFurniture.Items.ScrollShelf;
using KemyFurniture.Items.SeaChest;

namespace KemyFurniture.Core
{
    public static class ItemSetup
    {
        private static Shader cachedNativeStandard;

        public static void RegisterSaveIndex(GameObject prefab, int saveIndex)
        {
            if (prefab == null) return;

            var saveable = prefab.GetComponent<SaveablePrefab>() ?? prefab.AddComponent<SaveablePrefab>();
            saveable.prefabIndex = saveIndex;
        }

        public static void ConfigurePrefabProperties(GameObject prefab)
        {
            if (prefab == null) return;

            ApplyFogTest(prefab);

            string name = prefab.name.ToLower();
            var shipItem = prefab.GetComponent<ShipItem>() ?? prefab.AddComponent<ShipItem>();

            SanitizeRootColliders(prefab);

            // 1. Captain's Desk
            if (name.Contains("captaindesk") || name.Contains("captainsdesk"))
            {
                EnsureCrateComponents(prefab);
                shipItem.name = "Captain's Desk";
                shipItem.lookText = "Captain's Desk";
                if (prefab.GetComponent<CaptainsDeskLogic>() == null)
                {
                    prefab.AddComponent<CaptainsDeskLogic>();
                }
                SetItemValue(prefab, 750);
            }
            // 2. Small Cabinet
            else if (name.Contains("cabinetsmall"))
            {
                EnsureCrateComponents(prefab);
                shipItem.name = "Small Cabinet";
                shipItem.lookText = "Small Cabinet";
                if (prefab.GetComponent<CabinetSmallLogic>() == null)
                {
                    prefab.AddComponent<CabinetSmallLogic>();
                }
                SetItemValue(prefab, 480);
            }
            // 3. Wide Cabinet
            else if (name.Contains("cabinetwide"))
            {
                EnsureCrateComponents(prefab);
                shipItem.name = "Wide Cabinet";
                shipItem.lookText = "Wide Cabinet";
                if (prefab.GetComponent<CabinetWideLogic>() == null)
                {
                    prefab.AddComponent<CabinetWideLogic>();
                }
                SetItemValue(prefab, 720);
            }
            // 4. Large Cabinet
            else if (name.Contains("cabinet"))
            {
                EnsureCrateComponents(prefab);
                shipItem.name = "Cabinet";
                shipItem.lookText = "Cabinet";
                if (prefab.GetComponent<CabinetLogic>() == null)
                {
                    prefab.AddComponent<CabinetLogic>();
                }
                SetItemValue(prefab, 1200);
            }
            // 5. Chest
            else if (name.Contains("chest") || name.Contains("seachest"))
            {
                EnsureCrateComponents(prefab);
                shipItem.name = "Chest";
                shipItem.lookText = "Chest";
                if (prefab.GetComponent<SeaChestLogic>() == null)
                {
                    prefab.AddComponent<SeaChestLogic>();
                }
                SetItemValue(prefab, 800);
            }
            // 6. Scroll Shelf
            else if (name.Contains("scroll") || name.Contains("shelf"))
            {
                EnsureCrateComponents(prefab);
                shipItem.name = "Scroll Shelf";
                shipItem.lookText = "Scroll Shelf";
                if (prefab.GetComponent<ScrollShelfLogic>() == null)
                {
                    prefab.AddComponent<ScrollShelfLogic>();
                }
                SetItemValue(prefab, 450);
            }
            // 7. Carpets
            else if (name.Contains("carpet"))
            {
                string cleanName = "Red Carpet";
                if (name.Contains("blue")) cleanName = "Blue Carpet";
                else if (name.Contains("green")) cleanName = "Green Carpet";

                shipItem.name = cleanName;
                shipItem.lookText = cleanName;

                if (prefab.GetComponent<CarpetLogic>() == null)
                {
                    prefab.AddComponent<CarpetLogic>();
                }
                SetItemValue(prefab, 400);
            }
            // 8. Navigator's Desk
            else if (name.Contains("navigatordesk") || name.Contains("navigatortable") || name.Contains("table"))
            {
                shipItem.name = "Navigator's Desk";
                shipItem.lookText = "Navigator's Desk";

                if (prefab.GetComponent<NavigatorDeskLogic>() == null)
                {
                    prefab.AddComponent<NavigatorDeskLogic>();
                }
                SetItemValue(prefab, 650);
            }
            // 9. Bed
            else if (name.Contains("bed"))
            {
                shipItem.name = "Bed";
                shipItem.lookText = "Bed";

                if (prefab.GetComponent<BedLogic>() == null)
                {
                    prefab.AddComponent<BedLogic>();
                }
                SetItemValue(prefab, 950);
            }
        }

        private static void ApplyFogTest(GameObject go)
        {
            if (go == null) return;

            if (cachedNativeStandard == null)
            {
                cachedNativeStandard = Shader.Find("Standard");
            }

            if (cachedNativeStandard == null) return;

            foreach (Renderer renderer in go.GetComponentsInChildren<Renderer>(true))
            {
                Material[] mats = renderer.sharedMaterials;
                bool modified = false;

                for (int i = 0; i < mats.Length; i++)
                {
                    Material mat = mats[i];
                    if (mat == null) continue;

                    if (mat.shader != null && mat.shader.name.Contains("Standard"))
                    {
                        mat.shader = cachedNativeStandard;
                        modified = true;
                    }
                }

                if (modified)
                {
                    renderer.sharedMaterials = mats;
                }
            }
        }

        private static void EnsureCrateComponents(GameObject prefab)
        {
            if (prefab.GetComponent<ShipItemCrate>() == null)
            {
                prefab.AddComponent<ShipItemCrate>();
            }

            if (prefab.GetComponent<CrateInventory>() == null)
            {
                prefab.AddComponent<CrateInventory>();
            }

            var btn = prefab.GetComponent<GoPointerButton>() ?? prefab.AddComponent<GoPointerButton>();
            btn.enabled = true;
        }

        private static void SanitizeRootColliders(GameObject prefab)
        {
            var meshCols = prefab.GetComponentsInChildren<MeshCollider>(true);
            foreach (var col in meshCols)
            {
                if (!col.convex)
                {
                    col.convex = true;
                }
            }

            if (prefab.GetComponent<Collider>() == null)
            {
                prefab.AddComponent<BoxCollider>();
            }
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