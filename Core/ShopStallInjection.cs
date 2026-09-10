using HarmonyLib;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KemyFurniture
{
    public static class ShopStallInjection
    {
        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string sceneName = scene.name.ToLower();

            if (sceneName.Contains("main menu"))
            {
                return;
            }

            if (sceneName.Contains("gold rock"))
            {
                GetRunner().StartCoroutine(SpawnGoldRockShopRoutine());
            }
            else if (sceneName.Contains("dragon cliffs") || sceneName.Contains("island 9 e"))
            {
                GetRunner().StartCoroutine(SpawnDragonCliffsShopRoutine());
            }
            else if (sceneName.Contains("fort") || sceneName.Contains("island 15 m") || sceneName.Contains("aestrin"))
            {
                GetRunner().StartCoroutine(SpawnFortAestrinShopRoutine());
            }
        }

        private static CoroutineRunner GetRunner()
        {
            GameObject runner = GameObject.Find("FurnitureShopRunner");
            if (runner == null)
            {
                runner = new GameObject("FurnitureShopRunner");
                UnityEngine.Object.DontDestroyOnLoad(runner);
            }
            return runner.GetComponent<CoroutineRunner>() ?? runner.AddComponent<CoroutineRunner>();
        }

        private static GameObject ResolvePrefab(string targetName)
        {
            if (FurniturePlugin.LoadedPrefabs == null || FurniturePlugin.LoadedPrefabs.Length == 0) return null;
            string cleanTarget = targetName.Replace(" ", "").Replace("_", "").ToLower();

            // 1. Exact match priority
            GameObject exact = FurniturePlugin.LoadedPrefabs.FirstOrDefault(p =>
                p != null && p.name.Replace(" ", "").Replace("_", "").ToLower().Equals(cleanTarget, StringComparison.OrdinalIgnoreCase));

            if (exact != null) return exact;

            // 2. Substring fallback
            return FurniturePlugin.LoadedPrefabs.FirstOrDefault(p =>
                p != null && p.name.Replace(" ", "").Replace("_", "").ToLower().Contains(cleanTarget));
        }

        // =========================================================================
        // 1. GOLD ROCK CITY SHOP SETUP
        // =========================================================================
        private static IEnumerator SpawnGoldRockShopRoutine()
        {
            yield return new WaitForSeconds(2.5f);

            var sceneryRoot = GameObject.Find("island 1 A (gold rock) scenery");
            if (sceneryRoot == null) yield break;

            if (sceneryRoot.transform.Find("Kemy_GRC_Custom_ShopArea") != null) yield break;

            try
            {
                Vector3 stallPos = new Vector3(1537.722f, 7.300f, -367.737f);
                Quaternion stallRot = Quaternion.Euler(270.0f, 235.0f, 0.0f);

                Vector3 keeperPos = new Vector3(1536.423f, 5.497f, -366.105f);
                Quaternion keeperRot = Quaternion.Euler(0f, 145.9f, 0f);

                Vector3 shopAreaPos = new Vector3(1533.948f, 5.537f, -370.439f);
                Quaternion shopAreaRot = Quaternion.Euler(270.0f, 235.1f, 0.0f);
                Vector3 shopAreaSize = new Vector3(14.0f, 18.0f, 10.0f);

                ShopArea newShopArea = CreateClonedShop(
                    sceneryRoot,
                    prefix: "GRC",
                    templateStallName: "market_stall (8)",
                    templateShopAreaName: "shop",
                    templateShopkeeperName: "shopkeeper (6)",
                    stallPos: stallPos,
                    stallRot: stallRot,
                    shopAreaPos: shopAreaPos,
                    shopAreaRot: shopAreaRot,
                    shopAreaSize: shopAreaSize,
                    keeperPos: keeperPos,
                    keeperRot: keeperRot
                );

                GameObject cabinetAlAnkhPrefab = ResolvePrefab("CabinetAlAnkh") ?? ResolvePrefab("Cabinet");
                GameObject cabinetWideAlAnkhPrefab = ResolvePrefab("CabinetWideAlAnkh") ?? ResolvePrefab("CabinetWide");
                GameObject cabinetSmallAlAnkhPrefab = ResolvePrefab("CabinetSmallAlAnkh") ?? ResolvePrefab("CabinetSmall");
                GameObject chestAlAnkhPrefab = ResolvePrefab("ChestAlAnkh") ?? ResolvePrefab("SeaChest");
                GameObject bedAlAnkhPrefab = ResolvePrefab("BedAlAnkh") ?? ResolvePrefab("Bed");
                GameObject scrollShelfAlAnkhPrefab = ResolvePrefab("ScrollShelfAlAnkh") ?? ResolvePrefab("ScrollShelf");

                GameObject carpetPrefab = ResolvePrefab("Carpet");
                GameObject carpetBluePrefab = ResolvePrefab("CarpetBlue") ?? carpetPrefab;
                GameObject carpetGreenPrefab = ResolvePrefab("CarpetGreen") ?? carpetPrefab;
                GameObject navDeskAlAnkhPrefab = ResolvePrefab("NavigatorDeskAlAnkh") ?? ResolvePrefab("NavigatorTable");

                Transform parent = sceneryRoot.transform;

                CreateShopItemSpawner(parent, "Kemy_GRC_Cabinet_1", new Vector3(1527.726f, 5.641f, -371.374f), Quaternion.Euler(0.0f, 146.4f, 0.1f), cabinetAlAnkhPrefab);
                CreateShopItemSpawner(parent, "Kemy_GRC_CabinetWide", new Vector3(1528.834f, 5.550f, -372.561f), Quaternion.Euler(0.0f, 145.2f, 0.0f), cabinetWideAlAnkhPrefab);
                CreateShopItemSpawner(parent, "Kemy_GRC_CabinetSmall", new Vector3(1530.410f, 5.550f, -373.350f), Quaternion.Euler(0.0f, 146.7f, 0.0f), cabinetSmallAlAnkhPrefab);
                CreateShopItemSpawner(parent, "Kemy_GRC_NavigatorTable", new Vector3(1526.130f, 5.548f, -374.435f), Quaternion.Euler(0.0f, 236.1f, 0.0f), navDeskAlAnkhPrefab);
                CreateShopItemSpawner(parent, "Kemy_GRC_Bed_Lower", new Vector3(1531.235f, 5.914f, -370.532f), Quaternion.Euler(0.0f, 147.5f, 0.0f), bedAlAnkhPrefab);
                CreateShopItemSpawner(parent, "Kemy_GRC_Bed_Upper", new Vector3(1531.235f, 7.547f, -370.532f), Quaternion.Euler(0.0f, 147.5f, 0.0f), bedAlAnkhPrefab);
                CreateShopItemSpawner(parent, "Kemy_GRC_Chest_Lower", new Vector3(1532.814f, 6.313f, -368.393f), Quaternion.Euler(0.0f, 145.3f, 0.0f), chestAlAnkhPrefab);
                CreateShopItemSpawner(parent, "Kemy_GRC_Chest_Upper", new Vector3(1532.791f, 7.875f, -368.338f), Quaternion.Euler(0.0f, 146.8f, 0.0f), chestAlAnkhPrefab);
                CreateShopItemSpawner(parent, "Kemy_GRC_ScrollShelf_1", new Vector3(1534.890f, 6.398f, -369.552f), Quaternion.Euler(0.0f, 326.4f, 180.0f), scrollShelfAlAnkhPrefab);
                CreateShopItemSpawner(parent, "Kemy_GRC_ScrollShelf_2", new Vector3(1533.572f, 6.398f, -370.359f), Quaternion.Euler(0.0f, 324.7f, 180.0f), scrollShelfAlAnkhPrefab);

                CreateShopItemSpawner(parent, "Kemy_GRC_Carpet_Red", new Vector3(1535.796f, 5.577f, -372.323f), Quaternion.Euler(90.0f, 145.9f, 0.0f), carpetPrefab);
                CreateShopItemSpawner(parent, "Kemy_GRC_Carpet_Green", new Vector3(1537.736f, 5.537f, -369.456f), Quaternion.Euler(90.0f, 54.1f, 0.0f), carpetGreenPrefab);
                CreateShopItemSpawner(parent, "Kemy_GRC_Carpet_Blue", new Vector3(1539.450f, 5.537f, -368.251f), Quaternion.Euler(90.0f, 54.6f, 0.0f), carpetBluePrefab);

                FurniturePlugin.DiagLogger.LogInfo("[KEMY FURNITURE] Gold Rock City furniture stall verified/spawned.");
            }
            catch (Exception ex)
            {
                FurniturePlugin.DiagLogger.LogError($"[KEMY FURNITURE] Failed during GRC stall setup: {ex}");
            }
        }

        // =========================================================================
        // 2. DRAGON CLIFFS SHOP SETUP
        // =========================================================================
        private static IEnumerator SpawnDragonCliffsShopRoutine()
        {
            yield return new WaitForSeconds(2.5f);

            var sceneryRoot = GameObject.Find("island 9 E (dragon cliffs) scenery");
            if (sceneryRoot == null) yield break;

            if (sceneryRoot.transform.Find("Kemy_DC_Custom_ShopArea") != null) yield break;

            try
            {
                Vector3 keeperPos = new Vector3(-113.562f, 2.078f, -536.031f);
                Quaternion keeperRot = Quaternion.Euler(0f, 45.0f, 0f);

                Vector3 shopAreaPos = new Vector3(-112.270f, 1.999f, -538.193f);
                Quaternion shopAreaRot = Quaternion.Euler(270.0f, 43.0f, 0.0f);
                Vector3 shopAreaSize = new Vector3(10.64f, 5.05f, 6.5f);

                ShopArea newShopArea = CreateClonedShop(
                    sceneryRoot,
                    prefix: "DC",
                    templateStallName: null,
                    templateShopAreaName: "shop (1)",
                    templateShopkeeperName: "shopkeeper (1)",
                    stallPos: keeperPos,
                    stallRot: keeperRot,
                    shopAreaPos: shopAreaPos,
                    shopAreaRot: shopAreaRot,
                    shopAreaSize: shopAreaSize,
                    keeperPos: keeperPos,
                    keeperRot: keeperRot
                );

                GameObject cabinetPrefab = ResolvePrefab("Cabinet");
                GameObject cabinetSmallPrefab = ResolvePrefab("CabinetSmall");
                GameObject cabinetWidePrefab = ResolvePrefab("CabinetWide");
                GameObject bedPrefab = ResolvePrefab("Bed");
                GameObject chestPrefab = ResolvePrefab("SeaChest");
                GameObject carpetPrefab = ResolvePrefab("Carpet");
                GameObject carpetBluePrefab = ResolvePrefab("CarpetBlue") ?? carpetPrefab;
                GameObject carpetGreenPrefab = ResolvePrefab("CarpetGreen") ?? carpetPrefab;
                GameObject scrollShelfPrefab = ResolvePrefab("ScrollShelf");
                GameObject navigatorTablePrefab = ResolvePrefab("NavigatorTable");

                Transform parent = sceneryRoot.transform;

                CreateShopItemSpawner(parent, "Kemy_DC_NavigatorTable", new Vector3(-116.519f, 1.997f, -536.670f), Quaternion.Euler(271.3f, 181.8f, 131.4f), navigatorTablePrefab);
                CreateShopItemSpawner(parent, "Kemy_DC_ScrollShelf_Lower", new Vector3(-115.249f, 2.248f, -535.453f), Quaternion.Euler(1.1f, 134.7f, 90.4f), scrollShelfPrefab);
                CreateShopItemSpawner(parent, "Kemy_DC_ScrollShelf_Upper", new Vector3(-115.190f, 2.753f, -535.475f), Quaternion.Euler(1.1f, 134.5f, 90.4f), scrollShelfPrefab);
                CreateShopItemSpawner(parent, "Kemy_DC_Chest_Lower", new Vector3(-114.616f, 2.519f, -538.091f), Quaternion.Euler(3.8f, 42.4f, 0.0f), chestPrefab);
                CreateShopItemSpawner(parent, "Kemy_DC_Chest_Upper", new Vector3(-114.621f, 3.302f, -538.114f), Quaternion.Euler(3.8f, 42.4f, 0.0f), chestPrefab);
                CreateShopItemSpawner(parent, "Kemy_DC_Bed_Lower", new Vector3(-112.385f, 2.285f, -537.402f), Quaternion.Euler(0.0f, 134.1f, 0.0f), bedPrefab);
                CreateShopItemSpawner(parent, "Kemy_DC_Bed_Upper", new Vector3(-112.452f, 3.089f, -537.365f), Quaternion.Euler(0.1f, 134.9f, 358.5f), bedPrefab);

                CreateShopItemSpawner(parent, "Kemy_DC_Cabinet_1", new Vector3(-110.891f, 2.155f, -538.721f), Quaternion.Euler(0.3f, 45.1f, 359.8f), cabinetPrefab);
                CreateShopItemSpawner(parent, "Kemy_DC_Cabinet_2", new Vector3(-110.328f, 2.149f, -538.106f), Quaternion.Euler(0.0f, 43.3f, 0.1f), cabinetPrefab);
                CreateShopItemSpawner(parent, "Kemy_DC_CabinetSmall", new Vector3(-114.238f, 3.118f, -534.576f), Quaternion.Euler(0.0f, 44.3f, 0.0f), cabinetSmallPrefab);
                CreateShopItemSpawner(parent, "Kemy_DC_CabinetWide", new Vector3(-114.0f, 2.078f, -534.8f), keeperRot, cabinetWidePrefab);
                CreateShopItemSpawner(parent, "Kemy_DC_Carpet", new Vector3(-111.645f, 2.100f, -536.655f), Quaternion.Euler(101.2f, 46.3f, 0.2f), carpetPrefab);
                CreateShopItemSpawner(parent, "Kemy_DC_Carpet_Green", new Vector3(-111.449f, 1.983f, -535.442f), Quaternion.Euler(90.0f, 270.0f, 223.7f), carpetGreenPrefab);
                CreateShopItemSpawner(parent, "Kemy_DC_Carpet_Blue", new Vector3(-109.771f, 2.018f, -537.092f), Quaternion.Euler(90.0f, 119.1f, 73.4f), carpetBluePrefab);

                FurniturePlugin.DiagLogger.LogInfo("[KEMY FURNITURE] Dragon Cliffs furniture stall verified/spawned.");
            }
            catch (Exception ex)
            {
                FurniturePlugin.DiagLogger.LogError($"[KEMY FURNITURE] Failed during Dragon Cliffs setup: {ex}");
            }
        }

        // =========================================================================
        // 3. FORT AESTRIN SHOP SETUP
        // =========================================================================
        private static IEnumerator SpawnFortAestrinShopRoutine()
        {
            yield return new WaitForSeconds(2.5f);

            var sceneryRoot = GameObject.Find("island 15 M (Fort) scenery");
            if (sceneryRoot == null) yield break;

            if (sceneryRoot.transform.Find("Kemy_FA_Custom_ShopArea") != null) yield break;

            try
            {
                Vector3 stallPos = new Vector3(-113.321f, 2.099f, 44.512f);
                Quaternion stallRot = Quaternion.Euler(270.0f, 0.0f, 0.0f);

                Vector3 keeperPos = new Vector3(-113.203f, 2.099f, 42.729f);
                Quaternion keeperRot = Quaternion.Euler(0f, 0f, 0f);

                Vector3 shopAreaPos = stallPos;
                Quaternion shopAreaRot = Quaternion.Euler(270.0f, 0.0f, 0.0f);
                Vector3 shopAreaSize = new Vector3(10.0f, 10.0f, 7.0f);

                ShopArea newShopArea = CreateClonedShop(
                    sceneryRoot,
                    prefix: "FA",
                    templateStallName: "market stall medi 3 (1)",
                    templateShopAreaName: "shop area (3)",
                    templateShopkeeperName: "shopkeeper (3)",
                    stallPos: stallPos,
                    stallRot: stallRot,
                    shopAreaPos: shopAreaPos,
                    shopAreaRot: shopAreaRot,
                    shopAreaSize: shopAreaSize,
                    keeperPos: keeperPos,
                    keeperRot: keeperRot
                );

                GameObject cabinetPrefab = ResolvePrefab("Cabinet");
                GameObject cabinetSmallPrefab = ResolvePrefab("CabinetSmall");
                GameObject cabinetWidePrefab = ResolvePrefab("CabinetWide");
                GameObject bedPrefab = ResolvePrefab("Bed");
                GameObject chestPrefab = ResolvePrefab("SeaChest");
                GameObject carpetPrefab = ResolvePrefab("Carpet");
                GameObject carpetBluePrefab = ResolvePrefab("CarpetBlue") ?? carpetPrefab;
                GameObject carpetGreenPrefab = ResolvePrefab("CarpetGreen") ?? carpetPrefab;
                GameObject scrollShelfPrefab = ResolvePrefab("ScrollShelf");
                GameObject captainsDeskPrefab = ResolvePrefab("CaptainsDesk") ?? ResolvePrefab("CaptainDesk");
                GameObject navDeskAestrinPrefab = ResolvePrefab("NavigatorDeskAestrin") ?? ResolvePrefab("NavigatorDesk");

                Transform parent = sceneryRoot.transform;

                // Navigator Desk Aestrin: Snapped Y rotation from 111.4f to 90.0f
                CreateShopItemSpawner(parent, "Kemy_FA_NavigatorDeskAestrin", new Vector3(-115.431f, 2.129f, 43.826f), Quaternion.Euler(0.0f, 270.0f, 0.0f), navDeskAestrinPrefab);

                CreateShopItemSpawner(parent, "Kemy_FA_Chest_Lower", new Vector3(-115.441f, 2.514f, 45.624f), Quaternion.Euler(0.3f, 359.1f, 0.0f), chestPrefab);
                CreateShopItemSpawner(parent, "Kemy_FA_Chest_Upper", new Vector3(-115.441f, 3.294f, 45.624f), Quaternion.Euler(0.3f, 0.2f, 0.0f), chestPrefab);
                CreateShopItemSpawner(parent, "Kemy_FA_ScrollShelf_Lower", new Vector3(-112.023f, 2.362f, 46.443f), Quaternion.Euler(0.2f, 178.2f, 89.9f), scrollShelfPrefab);
                CreateShopItemSpawner(parent, "Kemy_FA_ScrollShelf_Upper", new Vector3(-112.016f, 2.864f, 46.444f), Quaternion.Euler(0.2f, 179.4f, 269.9f), scrollShelfPrefab);
                CreateShopItemSpawner(parent, "Kemy_FA_Bed_Lower", new Vector3(-111.126f, 2.313f, 45.245f), Quaternion.Euler(359.9f, 270.2f, 0.0f), bedPrefab);
                CreateShopItemSpawner(parent, "Kemy_FA_Bed_Upper", new Vector3(-111.126f, 3.130f, 45.245f), Quaternion.Euler(359.9f, 271.7f, 0.0f), bedPrefab);

                CreateShopItemSpawner(parent, "Kemy_FA_Cabinet_1", new Vector3(-111.617f, 2.164f, 43.250f), Quaternion.Euler(0.1f, 0.2f, 0.0f), cabinetPrefab);
                CreateShopItemSpawner(parent, "Kemy_FA_Cabinet_2", new Vector3(-111.350f, 2.164f, 44.200f), Quaternion.Euler(0.1f, 0.2f, 0.0f), cabinetPrefab);

                CreateShopItemSpawner(parent, "Kemy_FA_CabinetWide", new Vector3(-115.655f, 2.124f, 47.343f), Quaternion.Euler(0.3f, 359.3f, 0.0f), cabinetWidePrefab);
                CreateShopItemSpawner(parent, "Kemy_FA_CabinetSmall", new Vector3(-110.438f, 2.131f, 46.485f), Quaternion.Euler(359.8f, 1.1f, 359.9f), cabinetSmallPrefab);
                CreateShopItemSpawner(parent, "Kemy_FA_Carpet", new Vector3(-113.707f, 2.123f, 45.453f), Quaternion.Euler(270.0f, 359.4f, 0.0f), carpetPrefab);
                CreateShopItemSpawner(parent, "Kemy_FA_Carpet_Blue", new Vector3(-113.710f, 2.123f, 46.464f), Quaternion.Euler(89.8f, 179.0f, 359.3f), carpetBluePrefab);
                CreateShopItemSpawner(parent, "Kemy_FA_Carpet_Green", new Vector3(-113.665f, 2.123f, 47.494f), Quaternion.Euler(89.7f, 172.9f, 353.2f), carpetGreenPrefab);

                // Captain's Desk: Lowered Y from 2.496f to 2.129f to match ground level
                CreateShopItemSpawner(parent, "Kemy_FA_CaptainsDesk", new Vector3(-109.800f, 2.279f, 43.833f), Quaternion.Euler(0.0f, 90.0f, 0.0f), captainsDeskPrefab);
            }
            catch (Exception ex)
            {
                FurniturePlugin.DiagLogger.LogError($"[KEMY FURNITURE] Failed during Fort Aestrin setup: {ex}");
            }
        }

        // =========================================================================
        // CORE SPAWNING & CLONING HELPERS
        // =========================================================================
        private static ShopArea CreateClonedShop(
            GameObject scenery,
            string prefix,
            string templateStallName,
            string templateShopAreaName,
            string templateShopkeeperName,
            Vector3 stallPos,
            Quaternion stallRot,
            Vector3 shopAreaPos,
            Quaternion shopAreaRot,
            Vector3 shopAreaSize,
            Vector3 keeperPos,
            Quaternion keeperRot)
        {
            Transform[] allSceneryTransforms = scenery.GetComponentsInChildren<Transform>(true);

            if (!string.IsNullOrEmpty(templateStallName))
            {
                Transform stallTemplate = allSceneryTransforms.FirstOrDefault(t => t.name.Equals(templateStallName, StringComparison.OrdinalIgnoreCase))
                                       ?? allSceneryTransforms.FirstOrDefault(t => t.name.ToLower().Contains("stall") && t.name.ToLower().Contains("medi"));

                if (stallTemplate != null)
                {
                    GameObject clonedStall = UnityEngine.Object.Instantiate(stallTemplate.gameObject, scenery.transform);
                    clonedStall.name = $"Kemy_{prefix}_Custom_MarketStall_Mesh";
                    clonedStall.transform.localPosition = stallPos;
                    clonedStall.transform.localRotation = stallRot;

                    foreach (var r in clonedStall.GetComponentsInChildren<MeshRenderer>(true))
                    {
                        r.enabled = true;
                    }
                }
            }

            Transform shopAreaTemplate = allSceneryTransforms.FirstOrDefault(t => t.name.Equals(templateShopAreaName, StringComparison.OrdinalIgnoreCase))
                                      ?? allSceneryTransforms.FirstOrDefault(t => t.name.StartsWith("shop area", StringComparison.OrdinalIgnoreCase) && t.GetComponent<ShopArea>() != null)
                                      ?? allSceneryTransforms.FirstOrDefault(t => t.name.StartsWith("shop", StringComparison.OrdinalIgnoreCase) && t.GetComponent<ShopArea>() != null);

            if (shopAreaTemplate == null)
            {
                throw new Exception($"Could not find shop area trigger template matching '{templateShopAreaName}'");
            }

            GameObject clonedShop = UnityEngine.Object.Instantiate(shopAreaTemplate.gameObject, scenery.transform);
            clonedShop.name = $"Kemy_{prefix}_Custom_ShopArea";
            clonedShop.transform.localPosition = shopAreaPos;
            clonedShop.transform.localRotation = shopAreaRot;
            clonedShop.transform.localScale = Vector3.one;

            for (int i = clonedShop.transform.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(clonedShop.transform.GetChild(i).gameObject);
            }

            var rootMeshFilter = clonedShop.GetComponent<MeshFilter>();
            if (rootMeshFilter != null) UnityEngine.Object.Destroy(rootMeshFilter);
            var rootMeshRenderer = clonedShop.GetComponent<MeshRenderer>();
            if (rootMeshRenderer != null) UnityEngine.Object.Destroy(rootMeshRenderer);

            ShopArea shopArea = clonedShop.GetComponent<ShopArea>();
            if (shopArea != null && shopArea.itemsForSale != null)
            {
                shopArea.itemsForSale.Clear();
            }

            BoxCollider box = clonedShop.GetComponent<BoxCollider>();
            if (box != null)
            {
                box.center = new Vector3(0f, 0f, shopAreaSize.z * 0.5f);
                box.size = shopAreaSize;
            }

            Transform keeperTemplate = allSceneryTransforms.FirstOrDefault(t => t.name.Equals(templateShopkeeperName, StringComparison.OrdinalIgnoreCase))
                                    ?? allSceneryTransforms.FirstOrDefault(t => t.GetComponent<Shopkeeper>() != null);

            if (keeperTemplate == null)
            {
                throw new Exception($"Could not find Shopkeeper template matching '{templateShopkeeperName}'");
            }

            GameObject clonedKeeper = UnityEngine.Object.Instantiate(keeperTemplate.gameObject, scenery.transform);
            clonedKeeper.name = $"Kemy_{prefix}_Custom_Shopkeeper";
            clonedKeeper.transform.localPosition = keeperPos;
            clonedKeeper.transform.localRotation = keeperRot;

            for (int i = clonedKeeper.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = clonedKeeper.transform.GetChild(i);
                if (child.GetComponent<ShopItemSpawner>() != null || child.name.ToLower().Contains("item") || child.name.ToLower().Contains("stall"))
                {
                    UnityEngine.Object.Destroy(child.gameObject);
                }
            }

            Shopkeeper shopkeeper = clonedKeeper.GetComponent<Shopkeeper>();

            if (shopkeeper != null && shopArea != null)
            {
                AccessTools.Field(typeof(Shopkeeper), "shopLocalPos")?.SetValue(shopkeeper, stallPos);
                AccessTools.Field(typeof(Shopkeeper), "shopRotation")?.SetValue(shopkeeper, stallRot);
                AccessTools.Field(typeof(Shopkeeper), "shop")?.SetValue(shopkeeper, shopArea);
                AccessTools.Field(typeof(ShopArea), "keeper")?.SetValue(shopArea, shopkeeper);
            }

            return shopArea;
        }

        private static void CreateShopItemSpawner(Transform parent, string nodeName, Vector3 position, Quaternion rotation, GameObject prefab)
        {
            if (prefab == null) return;

            Transform existing = parent.Find(nodeName);
            if (existing != null)
            {
                UnityEngine.Object.Destroy(existing.gameObject);
            }

            GameObject spawnerNode = new GameObject(nodeName);
            spawnerNode.transform.parent = parent;
            spawnerNode.transform.localPosition = position;
            spawnerNode.transform.localRotation = rotation;

            MeshFilter filter = spawnerNode.AddComponent<MeshFilter>();
            var sourceFilter = prefab.GetComponent<MeshFilter>();
            if (sourceFilter != null)
            {
                filter.mesh = sourceFilter.mesh;
            }

            var renderer = spawnerNode.AddComponent<MeshRenderer>();
            renderer.enabled = false;

            ShopItemSpawner spawner = spawnerNode.AddComponent<ShopItemSpawner>();
            spawner.itemPrefab = prefab;
            spawner.priceMult = 1f;
        }
    }
}