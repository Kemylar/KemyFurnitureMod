using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KemyFurniture
{
    public static class ShopInjection
    {
        private static bool spawnedGRC_Table = false;
        private static bool spawnedGRC_Shelf = false;
        private static bool spawnedGRC_Chest = false;
        private static bool spawnedGRC_Bed = false;
        private static bool spawnedGRC_Carpet = false;
        private static bool spawnedGRC_Cabinet = false;

        private static bool spawnedDC_Table = false;
        private static bool spawnedDC_Shelf = false;
        private static bool spawnedDC_Chest = false;
        private static bool spawnedDC_Bed = false;
        private static bool spawnedDC_Carpet = false;
        private static bool spawnedDC_Cabinet = false;

        private static bool spawnedFT_Table = false;
        private static bool spawnedFT_Shelf = false;
        private static bool spawnedFT_Chest = false;
        private static bool spawnedFT_Bed = false;
        private static bool spawnedFT_Carpet = false;
        private static bool spawnedFT_Cabinet = false;

        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string sceneName = scene.name.ToLower();

            if (sceneName.Contains("main menu"))
            {
                spawnedGRC_Table = false;
                spawnedGRC_Shelf = false;
                spawnedGRC_Chest = false;
                spawnedGRC_Bed = false;
                spawnedGRC_Carpet = false;
                spawnedGRC_Cabinet = false;

                spawnedDC_Table = false;
                spawnedDC_Shelf = false;
                spawnedDC_Chest = false;
                spawnedDC_Bed = false;
                spawnedDC_Carpet = false;
                spawnedDC_Cabinet = false;

                spawnedFT_Table = false;
                spawnedFT_Shelf = false;
                spawnedFT_Chest = false;
                spawnedFT_Bed = false;
                spawnedFT_Carpet = false;
                spawnedFT_Cabinet = false;
                return;
            }

            GameObject runner = GameObject.Find("FurnitureShopItemSpawnerRunner");
            if (runner == null)
            {
                runner = new GameObject("FurnitureShopItemSpawnerRunner");
                UnityEngine.Object.DontDestroyOnLoad(runner);
            }
            var component = runner.GetComponent<CoroutineRunner>() ?? runner.AddComponent<CoroutineRunner>();

            // Fetch loaded prefabs directly from array indices matching PrefabDefinitions order
            if (FurniturePlugin.LoadedPrefabs == null || FurniturePlugin.LoadedPrefabs.Length < 6) return;

            GameObject tablePrefab = FurniturePlugin.LoadedPrefabs[0];
            GameObject shelfPrefab = FurniturePlugin.LoadedPrefabs[1];
            GameObject chestPrefab = FurniturePlugin.LoadedPrefabs[2];
            GameObject bedPrefab = FurniturePlugin.LoadedPrefabs[3];
            GameObject carpetPrefab = FurniturePlugin.LoadedPrefabs[4];
            GameObject cabinetPrefab = FurniturePlugin.LoadedPrefabs[5];

            // 1. Gold Rock City (GRC / Al Ankh Region)
            if (sceneName.Contains("gold rock"))
            {
                if (!spawnedGRC_Table)
                {
                    Vector3 pos = new Vector3(1549.292f, 5.106f, -359.500f);
                    Quaternion rot = Quaternion.Euler(270.6f, 208.3f, 24.5f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 1 A (gold rock) scenery", "GRC_Table", pos, rot, tablePrefab));
                }

                if (!spawnedGRC_Shelf)
                {
                    Vector3 pos = new Vector3(1553.299f, 5.946f, -363.415f);
                    Quaternion rot = Quaternion.Euler(359.5f, 50.3f, 179.8f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 1 A (gold rock) scenery", "GRC_Shelf", pos, rot, shelfPrefab));
                }

                if (!spawnedGRC_Chest)
                {
                    Vector3 pos = new Vector3(1552.333f, 5.871f, -361.843f);
                    Quaternion rot = Quaternion.Euler(0.5f, 229.8f, 0.2f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 1 A (gold rock) scenery", "GRC_Chest", pos, rot, chestPrefab));
                }

                if (!spawnedGRC_Cabinet)
                {
                    Vector3 pos = new Vector3(1557.326f, 5.113f, -368.731f);
                    Quaternion rot = Quaternion.Euler(0.3f, 233.3f, 1.0f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 1 A (gold rock) scenery", "GRC_Cabinet", pos, rot, cabinetPrefab));
                }

                if (!spawnedGRC_Bed)
                {
                    Vector3 pos = new Vector3(1561.968f, 5.470f, -362.938f);
                    Quaternion rot = Quaternion.Euler(0.4f, 234.0f, 1.0f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 1 A (gold rock) scenery", "GRC_Bed", pos, rot, bedPrefab));
                }

                if (!spawnedGRC_Carpet)
                {
                    Vector3 pos = new Vector3(1557.980f, 8.748f, -359.426f);
                    Quaternion rot = Quaternion.Euler(0.9f, 325.5f, 89.6f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 1 A (gold rock) scenery", "GRC_Carpet", pos, rot, carpetPrefab));
                }
            }

            // 2. Fort Aestrin (FT / FA)
            if (sceneName.Contains("fort"))
            {
                if (!spawnedFT_Table)
                {
                    Vector3 pos = new Vector3(-8.702f, 2.046f, 56.178f);
                    Quaternion rot = Quaternion.Euler(270.0f, 207.0f, 0.0f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 15 M (Fort) scenery", "FT_Table", pos, rot, tablePrefab));
                }

                if (!spawnedFT_Shelf)
                {
                    Vector3 pos = new Vector3(-11.002f, 2.474f, 57.059f);
                    Quaternion rot = Quaternion.Euler(0.0f, 26.5f, 180.0f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 15 M (Fort) scenery", "FT_Shelf", pos, rot, shelfPrefab));
                }

                if (!spawnedFT_Chest)
                {
                    Vector3 pos = new Vector3(-12.045f, 2.432f, 57.161f);
                    Quaternion rot = Quaternion.Euler(0.0f, 206.4f, 0.0f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 15 M (Fort) scenery", "FT_Chest", pos, rot, chestPrefab));
                }

                if (!spawnedFT_Bed)
                {
                    Vector3 pos = new Vector3(-8.564f, 2.232f, 59.899f);
                    Quaternion rot = Quaternion.Euler(0.0f, 204.6f, 0.0f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 15 M (Fort) scenery", "FT_Bed", pos, rot, bedPrefab));
                }

                if (!spawnedFT_Carpet)
                {
                    Vector3 pos = new Vector3(-9.178f, 4.675f, 56.915f);
                    Quaternion rot = Quaternion.Euler(281.3f, 208.3f, 356.0f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 15 M (Fort) scenery", "FT_Carpet", pos, rot, carpetPrefab));
                }

                if (!spawnedFT_Cabinet)
                {
                    Vector3 pos = new Vector3(-11.046f, 2.094f, 62.595f);
                    Quaternion rot = Quaternion.Euler(0.0f, 206.4f, 0.0f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 15 M (Fort) scenery", "FT_Cabinet", pos, rot, cabinetPrefab));
                }
            }

            // 3. Dragon Cliffs (DC)
            if (sceneName.Contains("dragon cliffs"))
            {
                if (!spawnedDC_Table)
                {
                    Vector3 pos = new Vector3(-88.111f, 3.606f, -539.981f);
                    Quaternion rot = Quaternion.Euler(270.0f, 316.0f, 0.0f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 9 E (dragon cliffs) scenery", "DC_Table", pos, rot, tablePrefab));
                }

                if (!spawnedDC_Shelf)
                {
                    Vector3 pos = new Vector3(-87.397f, 4.057f, -538.554f);
                    Quaternion rot = Quaternion.Euler(0.0f, 134.7f, 180.0f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 9 E (dragon cliffs) scenery", "DC_Shelf", pos, rot, shelfPrefab));
                }

                if (!spawnedDC_Chest)
                {
                    Vector3 pos = new Vector3(-82.608f, 4.014f, -546.205f);
                    Quaternion rot = Quaternion.Euler(0.0f, 225.8f, 0.0f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 9 E (dragon cliffs) scenery", "DC_Chest", pos, rot, chestPrefab));
                }

                if (!spawnedDC_Bed)
                {
                    Vector3 pos = new Vector3(-80.237f, 3.815f, -545.822f);
                    Quaternion rot = Quaternion.Euler(0.0f, 225.8f, 0.0f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 9 E (dragon cliffs) scenery", "DC_Bed", pos, rot, bedPrefab));
                }

                if (!spawnedDC_Carpet)
                {
                    Vector3 pos = new Vector3(-84.013f, 4.585f, -540.459f);
                    Quaternion rot = Quaternion.Euler(0.0f, 134.9f, 278.9f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 9 E (dragon cliffs) scenery", "DC_Carpet", pos, rot, carpetPrefab));
                }

                if (!spawnedDC_Cabinet)
                {
                    Vector3 pos = new Vector3(-81.324f, 3.676f, -547.024f);
                    Quaternion rot = Quaternion.Euler(0.0f, 226.3f, 0.0f);
                    component.StartCoroutine(DelayedInjectionRoutine(
                        "island 9 E (dragon cliffs) scenery", "DC_Cabinet", pos, rot, cabinetPrefab));
                }
            }
        }

        private static IEnumerator DelayedInjectionRoutine(string parentSceneryName, string regionKey, Vector3 localPos, Quaternion localRot, GameObject prefab)
        {
            yield return new WaitForSeconds(3f);

            if (prefab == null)
            {
                FurniturePlugin.DiagLogger.LogError($"[KEMY FURNITURE] Aborting spawner injection for {regionKey}: Prefab reference is NULL!");
                yield break;
            }

            if (regionKey == "GRC_Table" && spawnedGRC_Table) yield break;
            if (regionKey == "GRC_Shelf" && spawnedGRC_Shelf) yield break;
            if (regionKey == "GRC_Chest" && spawnedGRC_Chest) yield break;
            if (regionKey == "GRC_Bed" && spawnedGRC_Bed) yield break;
            if (regionKey == "GRC_Carpet" && spawnedGRC_Carpet) yield break;
            if (regionKey == "GRC_Cabinet" && spawnedGRC_Cabinet) yield break;

            if (regionKey == "DC_Table" && spawnedDC_Table) yield break;
            if (regionKey == "DC_Shelf" && spawnedDC_Shelf) yield break;
            if (regionKey == "DC_Chest" && spawnedDC_Chest) yield break;
            if (regionKey == "DC_Bed" && spawnedDC_Bed) yield break;
            if (regionKey == "DC_Carpet" && spawnedDC_Carpet) yield break;
            if (regionKey == "DC_Cabinet" && spawnedDC_Cabinet) yield break;

            if (regionKey == "FT_Table" && spawnedFT_Table) yield break;
            if (regionKey == "FT_Shelf" && spawnedFT_Shelf) yield break;
            if (regionKey == "FT_Chest" && spawnedFT_Chest) yield break;
            if (regionKey == "FT_Bed" && spawnedFT_Bed) yield break;
            if (regionKey == "FT_Carpet" && spawnedFT_Carpet) yield break;
            if (regionKey == "FT_Cabinet" && spawnedFT_Cabinet) yield break;

            var sceneryRoot = GameObject.Find(parentSceneryName);
            if (sceneryRoot == null) yield break;

            try
            {
                GameObject spawnerNode = new GameObject($"shop item spawner ({prefab.name} {regionKey})");
                spawnerNode.transform.parent = sceneryRoot.transform;
                spawnerNode.transform.localPosition = localPos;
                spawnerNode.transform.localRotation = localRot;

                var nativeSpawner = spawnerNode.AddComponent<ShopItemSpawner>();
                nativeSpawner.itemPrefab = prefab;

                if (regionKey == "GRC_Table") spawnedGRC_Table = true;
                if (regionKey == "GRC_Shelf") spawnedGRC_Shelf = true;
                if (regionKey == "GRC_Chest") spawnedGRC_Chest = true;
                if (regionKey == "GRC_Bed") spawnedGRC_Bed = true;
                if (regionKey == "GRC_Carpet") spawnedGRC_Carpet = true;
                if (regionKey == "GRC_Cabinet") spawnedGRC_Cabinet = true;

                if (regionKey == "DC_Table") spawnedDC_Table = true;
                if (regionKey == "DC_Shelf") spawnedDC_Shelf = true;
                if (regionKey == "DC_Chest") spawnedDC_Chest = true;
                if (regionKey == "DC_Bed") spawnedDC_Bed = true;
                if (regionKey == "DC_Carpet") spawnedDC_Carpet = true;
                if (regionKey == "DC_Cabinet") spawnedDC_Cabinet = true;

                if (regionKey == "FT_Table") spawnedFT_Table = true;
                if (regionKey == "FT_Shelf") spawnedFT_Shelf = true;
                if (regionKey == "FT_Chest") spawnedFT_Chest = true;
                if (regionKey == "FT_Bed") spawnedFT_Bed = true;
                if (regionKey == "FT_Carpet") spawnedFT_Carpet = true;
                if (regionKey == "FT_Cabinet") spawnedFT_Cabinet = true;

                FurniturePlugin.DiagLogger.LogInfo($"[KEMY FURNITURE] {prefab.name} cleanly deployed at {regionKey} via ShopItemSpawner anchor.");
            }
            catch (Exception ex)
            {
                FurniturePlugin.DiagLogger.LogError($"[KEMY FURNITURE] Failed injection for {prefab.name} at {regionKey}: {ex}");
            }
        }
    }

    public class CoroutineRunner : MonoBehaviour { }
}