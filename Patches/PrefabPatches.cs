using HarmonyLib;
using System;
using UnityEngine;

namespace KemyFurniture
{
    [HarmonyPatch(typeof(PrefabsDirectory), "PopulateShipItems")]
    public static class PreloadDirectoryPatch
    {
        // Unique ID slot for the table. 900 is generally safe from vanilla conflicts.
        public const int NAV_TABLE_INDEX = 900;

        private static GameObject navigatorTablePrefab;

        // Clean reference exposed for your ShopInjection script
        public static GameObject NavigatorTablePrefabRef => navigatorTablePrefab;

        [HarmonyPrefix]
        public static void Prefix(PrefabsDirectory __instance)
        {
            if (__instance.directory == null) return;

            // Dynamically resize the array safely if our index exceeds its capacity
            if (__instance.directory.Length <= NAV_TABLE_INDEX)
            {
                Array.Resize(ref __instance.directory, NAV_TABLE_INDEX + 10);
            }
        }

        [HarmonyPostfix]
        public static void Postfix(PrefabsDirectory __instance)
        {
            if (FurniturePlugin.MainAssetBundle == null) return;

            try
            {
                // Load & configure the NavigatorTable asset from the bundle
                if (navigatorTablePrefab == null)
                {
                    // Must match the literal asset name inside your Unity Project window / Prefab folder
                    navigatorTablePrefab = FurniturePlugin.MainAssetBundle.LoadAsset<GameObject>("NavigatorTable");

                    // Register physics, economy values, and save state properties dynamically
                    FurnitureSetup.Configure(
                        navigatorTablePrefab, // 1
                        NAV_TABLE_INDEX,      // 2
                        "navigator table",    // 3
                        300,                  // 4 (Price)
                        25.0f                 // 5 (Mass - or whatever baseline mass value you want to test next)
                    );
                }

                // Inject the configured prefab into the targeted index slot
                __instance.directory[NAV_TABLE_INDEX] = navigatorTablePrefab;
            }
            catch (Exception ex)
            {
                FurniturePlugin.DiagLogger.LogError($"[KEMY FURNITURE] Postfix directory mapping failed: {ex}");
            }
        }
    }

    // =========================================================================
    // HARMONY PATCH: KEMY FURNITURE AUTO-SETTLE RUNTIME
    // =========================================================================
    [HarmonyPatch(typeof(global::StartMenu), "Update")]
    public static class FurnitureKinematicUnclockerPatch
    {
        private static float settlementTimer = 0f;
        private static bool settleCheckCompleted = false;

        [HarmonyPostfix]
        public static void Postfix()
        {
            if (settleCheckCompleted) return;

            // Give the world space 3 seconds after loading animation completes to finish structural generation
            settlementTimer += Time.deltaTime;
            if (settlementTimer < 3.0f) return;

            // Target the active world runtime clone of your desk
            GameObject tableInstance = GameObject.Find("NavigatorTable(Clone)");
            if (tableInstance != null)
            {
                var shipItem = tableInstance.GetComponent<ShipItem>();

                // If the game loaded it as unsold or unparented at the scene root, manually anchor it
                if (tableInstance.transform.parent == null && shipItem != null)
                {
                    Transform sceneryRoot = GameObject.Find("_shifting world")?.transform;
                    if (sceneryRoot != null)
                    {
                        tableInstance.transform.SetParent(sceneryRoot, true);
                        FurniturePlugin.DiagLogger.LogInfo("[KEMY FURNITURE] Successfully force-anchored orphaned table clone to _shifting world root.");
                    }
                }

                // Break the kinematic loading lock so the shadow proxy Rigidbody drops onto the deck naturally
                Rigidbody rb = tableInstance.GetComponentInChildren<Rigidbody>();
                if (rb != null && rb.isKinematic)
                {
                    FurniturePlugin.DiagLogger.LogWarning("[KEMY FURNITURE] Releasing loading hold: Un-setting Kinematic status to resolve precision pop.");
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }
            }

            settleCheckCompleted = true;
        }

        public static void ResetSettleState()
        {
            settleCheckCompleted = false;
            settlementTimer = 0f;
        }
    }

    // =========================================================================
    // HARMONY PATCH: RESET TRACKER STATUS ACROSS SAVE LIFECYCLES
    // =========================================================================
    [HarmonyPatch(typeof(global::StartMenu), "LoadGame")]
    public static class SaveReloadResetHook
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            FurnitureKinematicUnclockerPatch.ResetSettleState();
        }
    }
}