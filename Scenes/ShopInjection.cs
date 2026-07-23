using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KemyFurniture
{
    public static class ShopInjection
    {
        private static bool spawnedGRC = false;

        public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string sceneName = scene.name.ToLower();

            if (sceneName.Contains("main menu"))
            {
                spawnedGRC = false;
                return;
            }

            GameObject runner = GameObject.Find("FurnitureShopItemSpawnerRunner");
            if (runner == null)
            {
                runner = new GameObject("FurnitureShopItemSpawnerRunner");
                UnityEngine.Object.DontDestroyOnLoad(runner);
            }
            var component = runner.GetComponent<CoroutineRunner>() ?? runner.AddComponent<CoroutineRunner>();

            if (sceneName.Contains("gold rock") && !spawnedGRC)
            {
                component.StartCoroutine(DelayedInjectionRoutine(
                    "island 1 A (gold rock) scenery",
                    "GRC",
                    new Vector3(1550.786f, 5.055f, -360.993f),
                    Quaternion.Euler(-90.0f, 140.1f, 0.0f),
                    PreloadDirectoryPatch.NavigatorTablePrefabRef
                ));
            }
        }

        private static IEnumerator DelayedInjectionRoutine(string parentSceneryName, string regionKey, Vector3 localPos, Quaternion localRot, GameObject prefab)
        {
            yield return new WaitForSeconds(3f);

            if (prefab == null || (regionKey == "GRC" && spawnedGRC)) yield break;

            var sceneryRoot = GameObject.Find(parentSceneryName);
            if (sceneryRoot == null) yield break;

            try
            {
                GameObject spawnerNode = new GameObject($"shop item spawner ({prefab.name} {regionKey})");
                spawnerNode.transform.parent = sceneryRoot.transform;
                spawnerNode.transform.localPosition = localPos;
                spawnerNode.transform.localRotation = localRot;

                // FIX: Removed the manual MeshFilter/MeshRenderer generation entirely.
                // Let the native ShopItemSpawner handle the visual clone instantiation safely.
                var nativeSpawner = spawnerNode.AddComponent<ShopItemSpawner>();
                nativeSpawner.itemPrefab = prefab;

                if (regionKey == "GRC") spawnedGRC = true;

                FurniturePlugin.DiagLogger.LogInfo($"[KEMY FURNITURE] {prefab.name} cleanly deployed at {regionKey} via clean ShopItemSpawner anchor.");
            }
            catch (Exception ex)
            {
                FurniturePlugin.DiagLogger.LogError($"[KEMY FURNITURE] Failed injection for {prefab.name} at {regionKey}: {ex}");
            }
        }
    }

    public class CoroutineRunner : MonoBehaviour { }
}