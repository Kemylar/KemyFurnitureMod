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

            string name = prefab.name.ToLower().Replace(" ", "").Replace("_", "");
            var shipItem = prefab.GetComponent<ShipItem>() ?? prefab.AddComponent<ShipItem>();

            SanitizeRootColliders(prefab);

            // =====================================================================
            // 1. ASSIGN PREFAB SAVE INDICES (450 - 480 MASTER REGISTRY)
            // =====================================================================
            int assignedId = ResolveMasterId(name);
            if (assignedId > 0)
            {
                RegisterSaveIndex(prefab, assignedId);
            }

            // =====================================================================
            // 2. CONFIGURE BEHAVIOURS, NAMES, AND PRICES
            // =====================================================================

            // Captain's Desk (466)
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
            // Small Cabinets (458, 462, 470)
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
            // Wide Cabinets (459, 461, 471)
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
            // Large Cabinets (455, 460, 469)
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
            // Chests (452, 463, 472)
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
            // Scroll Shelves (451, 465, 474)
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
            // Carpets (454, 456, 457)
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
            // Navigator's Desks (450, 467, 468, 475)
            else if (name.Contains("navigatordesk") || name.Contains("navigatortable") || name.Contains("navtable") || name.Contains("table"))
            {
                shipItem.name = "Navigator's Desk";
                shipItem.lookText = "Navigator's Desk";

                if (prefab.GetComponent<NavigatorDeskLogic>() == null)
                {
                    prefab.AddComponent<NavigatorDeskLogic>();
                }
                SetItemValue(prefab, 650);
            }
            // Beds (453, 464, 473)
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
            // Cushions (479, 480)
            else if (name.Contains("cushion"))
            {
                string cName = name.Contains("white") ? "White Cushion" : "Red Cushion";
                shipItem.name = cName;
                shipItem.lookText = cName;
                SetItemValue(prefab, 400);
            }
            // Chair EA (478)
            else if (name.Contains("chairea") || name.Contains("chair"))
            {
                shipItem.name = "Chair";
                shipItem.lookText = "Chair";
                SetItemValue(prefab, 480);
            }
            // Room Divider (477)
            else if (name.Contains("divider"))
            {
                shipItem.name = "Room Divider";
                shipItem.lookText = "Room Divider";
                SetItemValue(prefab, 480);
            }
        }

        private static int ResolveMasterId(string cleanName)
        {
            // Baseline 450 - 459
            if (cleanName == "navigatortable" || cleanName == "navtable") return 450;
            if (cleanName == "scrollshelf") return 451;
            if (cleanName == "seachest") return 452;
            if (cleanName == "bed") return 453;
            if (cleanName == "carpet" || cleanName == "carpetred") return 454;
            if (cleanName == "cabinet") return 455;
            if (cleanName == "carpetblue") return 456;
            if (cleanName == "carpetgreen") return 457;
            if (cleanName == "cabinetsmall") return 458;
            if (cleanName == "cabinetwide") return 459;

            // Al'Ankh 460 - 465
            if (cleanName == "cabinetalankh" || cleanName == "cabinetaa") return 460;
            if (cleanName == "cabinetwidealankh" || cleanName == "cabinetwideaa") return 461;
            if (cleanName == "cabinetsmallalankh" || cleanName == "cabinetsmallaa") return 462;
            if (cleanName == "chestalankh" || cleanName == "chestaa") return 463;
            if (cleanName == "bedalankh" || cleanName == "bedaa") return 464;
            if (cleanName == "scrollshelfalankh" || cleanName == "scrollshelfaa") return 465;

            // Aestrin & Desks 466 - 468
            if (cleanName == "captainsdesk" || cleanName == "captaindesk") return 466;
            if (cleanName == "navigatordesk" || cleanName == "navdesk") return 467;
            if (cleanName == "navigatordeskalankh" || cleanName == "navdeskaa") return 468;

            // Emerald Archipelago 469 - 475
            if (cleanName == "cabinetea") return 469;
            if (cleanName == "cabinetsmallea") return 470;
            if (cleanName == "cabinetwideea") return 471;
            if (cleanName == "chestea") return 472;
            if (cleanName == "bedea") return 473;
            if (cleanName == "scrollshelfea") return 474;
            if (cleanName == "navigatordeskea" || cleanName == "navdeskea") return 475;

            // 476 Open Buffer
            // New Batch 477 - 480
            if (cleanName == "divider") return 477;
            if (cleanName == "chairea") return 478;
            if (cleanName == "cushionred") return 479;
            if (cleanName == "cushionwhite") return 480;

            return -1;
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