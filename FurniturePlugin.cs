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

        private void Awake()
        {
            DiagLogger = Logger;

            // 1. Load the AssetBundle cleanly from the mod folder
            string modDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string bundlePath = Path.Combine(modDirectory, "kemyfurnitureassets"); // Your asset bundle file name

            if (File.Exists(bundlePath))
            {
                MainAssetBundle = AssetBundle.LoadFromFile(bundlePath);
                DiagLogger.LogInfo("[KEMY FURNITURE] AssetBundle loaded successfully.");
            }
            else
            {
                DiagLogger.LogError($"[KEMY FURNITURE] Critical Error: AssetBundle not found at {bundlePath}");
            }

            // 2. Initialize and apply Harmony patches
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

            // 3. Register for scene load events to inject items into shops
            SceneManager.sceneLoaded += ShopInjection.OnSceneLoaded;
        }

        private void OnDestroy()
        {
            // Clean up event subscription on unload
            SceneManager.sceneLoaded -= ShopInjection.OnSceneLoaded;
        }
    }
}