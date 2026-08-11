using HarmonyLib;
using System;
using UnityEngine;

namespace KemyFurniture.Core
{
    // 1. GLOBAL PREFAB DIRECTORY INJECTION
    [HarmonyPatch(typeof(PrefabsDirectory), "PopulateShipItems")]
    public static class FurnitureDirectoryInjectionPatch
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            var prefabs = FurniturePlugin.LoadedPrefabs;
            if (prefabs == null || prefabs.Length == 0) return;

            try
            {
                foreach (var prefab in prefabs)
                {
                    if (prefab == null) continue;

                    SaveablePrefab saveComp = prefab.GetComponent<SaveablePrefab>();
                    if (saveComp == null) continue;

                    int index = saveComp.prefabIndex;

                    // Dynamically resize directory if index is larger than current capacity
                    if (PrefabsDirectory.instance.directory.Length <= index)
                    {
                        Array.Resize(ref PrefabsDirectory.instance.directory, index + 1);
                    }

                    PrefabsDirectory.instance.directory[index] = prefab;
                    FurniturePlugin.DiagLogger.LogInfo($"[KEMY FURNITURE] {prefab.name} registered cleanly at slot {index}");
                }
            }
            catch (Exception ex)
            {
                FurniturePlugin.DiagLogger.LogError("[KEMY FURNITURE] Critical Failure during directory injection: " + ex);
            }
        }
    }

    // 2. INTERCEPT LOOK UI PROMPTS
    [HarmonyPatch(typeof(LookUI), nameof(LookUI.ShowLookText))]
    public static class LookUIShowTextInjectionPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(LookUI __instance, GoPointerButton button)
        {
            ShipItem component = button.GetComponent<ShipItem>();
            if (component == null || !component.sold) return true;

            PickupableItem held = null;
            object pointerObj = AccessTools.Field(typeof(GoPointerButton), "pointedAtBy").GetValue(button);
            if (pointerObj is GoPointer pointer)
            {
                held = pointer.GetHeldItem();
            }

            if (held != null && held.GetComponent<ShipItemHammer>() != null) return true;

            var customLogic = component.GetComponent<ICustomFurnitureLogic>();
            if (customLogic != null && customLogic.OverrideLookUI)
            {
                __instance.ClearText();

                if (!component.nailed) AccessTools.Method(typeof(LookUI), "ShowLicon").Invoke(__instance, null);
                AccessTools.Method(typeof(LookUI), "ShowRicon").Invoke(__instance, null);

                var extraTextField = AccessTools.Field(typeof(LookUI), "extraText").GetValue(__instance);
                var controlsTextField = AccessTools.Field(typeof(LookUI), "controlsText").GetValue(__instance);

                if (extraTextField != null) AccessTools.Property(extraTextField.GetType(), "text").SetValue(extraTextField, component.lookText);

                if (controlsTextField != null)
                {
                    string promptText = component.nailed ? customLogic.CustomControlPrompt : $"Pick Up\n{customLogic.CustomControlPrompt}";
                    AccessTools.Property(controlsTextField.GetType(), "text").SetValue(controlsTextField, promptText);
                }

                __instance.transform.position = button.gameObject.transform.position;
                __instance.transform.LookAt(Camera.main.transform);

                if (Settings.hintTextEnabled)
                {
                    var hintTextField = AccessTools.Field(typeof(LookUI), "hintText").GetValue(__instance);
                    if (hintTextField != null) AccessTools.Property(hintTextField.GetType(), "text").SetValue(hintTextField, button.description);
                    AccessTools.Field(typeof(LookUI), "currentButton").SetValue(__instance, button);
                }

                return false;
            }

            return true;
        }
    }

    // 3. INTERCEPT INTERACTION (ALT-ACTIVATE)
    [HarmonyPatch(typeof(ShipItem), nameof(ShipItem.OnAltActivate), new Type[0])]
    public static class ShipItemAltActivationPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ShipItem __instance)
        {
            if (!__instance.sold) return true;

            var customLogic = __instance.GetComponent<ICustomFurnitureLogic>();
            return customLogic != null ? customLogic.OnAltActivate(__instance) : true;
        }
    }

    // 4. INTERCEPT RECTANGULAR STORAGE GRID DIMENSIONS
    [HarmonyPatch(typeof(CrateInventoryUI), "GetCrateDimensions")]
    public static class CrateInventoryUIOverridePatch
    {
        [HarmonyPostfix]
        public static void Postfix(CrateInventoryUI __instance, ref Vector2 __result)
        {
            if (__instance.currentCrate != null && __instance.currentCrate.gameObject != null)
            {
                var customLogic = __instance.currentCrate.GetComponent<ICustomFurnitureLogic>();
                if (customLogic != null && customLogic.HasCustomGrid)
                {
                    __result = customLogic.GridDimensions;
                }
            }
        }
    }
}