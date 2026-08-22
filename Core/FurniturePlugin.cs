using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KemyFurniture
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class FurniturePlugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.kemy.kemyfurniture";
        public const string PLUGIN_NAME = "Kemy's Furniture";
        public const string PLUGIN_VERSION = "1.2.0";

        public static AssetBundle MainAssetBundle { get; private set; }
        public static GameObject[] LoadedPrefabs { get; private set; }
        public static ManualLogSource DiagLogger { get; private set; }

        private void Awake()
        {
            DiagLogger = Logger;

            // 1. Load AssetBundle
            string modDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string bundlePath = Path.Combine(modDirectory, "kemyfurnitureassets");

            if (File.Exists(bundlePath))
            {
                MainAssetBundle = AssetBundle.LoadFromFile(bundlePath);
                DiagLogger.LogInfo("[KEMY FURNITURE] AssetBundle loaded successfully.");
            }
            else
            {
                DiagLogger.LogError($"[KEMY FURNITURE] Critical Error: AssetBundle not found at {bundlePath}");
                return;
            }

            // 2. Extract and configure prefabs
            LoadedPrefabs = MainAssetBundle.LoadAllAssets<GameObject>();

            if (LoadedPrefabs != null)
            {
                foreach (var prefab in LoadedPrefabs)
                {
                    if (prefab == null) continue;
                    Core.ItemSetup.ConfigurePrefabProperties(prefab);
                }
            }

            // 3. Apply Harmony patches
            try
            {
                var harmony = new Harmony(PLUGIN_GUID);
                harmony.PatchAll();
                DiagLogger.LogInfo("[KEMY FURNITURE] Harmony patches applied successfully.");
            }
            catch (Exception ex)
            {
                DiagLogger.LogError($"[KEMY FURNITURE] Failed to apply patches: {ex}");
            }

            // 4. Register scene hook for custom shop spawning
            SceneManager.sceneLoaded += ShopStallInjection.OnSceneLoaded;
        }

        public static void ForceDirectDirectoryInjection()
        {
            if (LoadedPrefabs == null || LoadedPrefabs.Length == 0) return;

            try
            {
                foreach (var prefab in LoadedPrefabs)
                {
                    if (prefab == null) continue;

                    var saveComp = prefab.GetComponent<SaveablePrefab>();
                    if (saveComp == null) continue;

                    int index = saveComp.prefabIndex;
                    if (PrefabsDirectory.instance.directory.Length <= index)
                    {
                        Array.Resize(ref PrefabsDirectory.instance.directory, index + 1);
                    }

                    PrefabsDirectory.instance.directory[index] = prefab;
                }
            }
            catch (Exception ex)
            {
                DiagLogger.LogError("[KEMY FURNITURE] Critical Failure during directory injection: " + ex);
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= ShopStallInjection.OnSceneLoaded;
        }
    }
}