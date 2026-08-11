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
        public const string PLUGIN_VERSION = "1.0.0";

        public static AssetBundle MainAssetBundle { get; private set; }
        public static ManualLogSource DiagLogger { get; private set; }
        public static GameObject[] LoadedPrefabs { get; private set; }

        private static readonly (string name, int index)[] PrefabDefinitions = new[]
        {
            ("NavigatorTable", 450),
            ("ScrollShelf",    451),
            ("SeaChest",       452),
            ("Bed",            453),
            ("Carpet",         454),
            ("Cabinet",        455)
        };

        private void Awake()
        {
            DiagLogger = Logger;

            string modDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string bundlePath = Path.Combine(modDirectory, "kemyfurnitureassets");

            if (File.Exists(bundlePath))
            {
                MainAssetBundle = AssetBundle.LoadFromFile(bundlePath);
                DiagLogger.LogInfo("[KEMY FURNITURE] AssetBundle loaded successfully.");

                LoadedPrefabs = new GameObject[PrefabDefinitions.Length];

                for (int i = 0; i < PrefabDefinitions.Length; i++)
                {
                    var (name, index) = PrefabDefinitions[i];
                    GameObject prefab = MainAssetBundle.LoadAsset<GameObject>(name);

                    if (prefab != null)
                    {
                        Core.ItemSetup.RegisterSaveIndex(prefab, index);
                        LoadedPrefabs[i] = prefab;
                        DiagLogger.LogInfo($"[KEMY FURNITURE] Loaded and configured {name}.");
                    }
                    else
                    {
                        DiagLogger.LogError($"[KEMY FURNITURE] Failed to extract {name} from AssetBundle!");
                    }
                }
            }
            else
            {
                DiagLogger.LogError($"[KEMY FURNITURE] Critical Error: AssetBundle not found at {bundlePath}");
            }

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

            SceneManager.sceneLoaded += ShopInjection.OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= ShopInjection.OnSceneLoaded;
        }
    }
}