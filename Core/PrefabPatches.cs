using HarmonyLib;
using System;
using UnityEngine;

namespace KemyFurniture.Core
{
    // 1. PREVENT TOGGLECOLLIDER CRASHES
    [HarmonyPatch(typeof(ItemRigidbody), "ToggleCollider")]
    public static class ItemRigidbodyNREGuard
    {
        [HarmonyFinalizer]
        public static Exception Finalizer(Exception __exception) => null;
    }

    // 2. PREVENT UPDATEMASS NRE ON RESTOCK
    [HarmonyPatch(typeof(ItemRigidbody), "UpdateMass")]
    public static class ItemRigidbodyMassGuard
    {
        [HarmonyFinalizer]
        public static Exception Finalizer(Exception __exception) => null;
    }

    // 3. PREVENT SHIPITEMCRATE ONLOAD NRE ON EMPTY FURNITURE RESTOCK
    [HarmonyPatch(typeof(ShipItemCrate), "OnLoad")]
    public static class ShipItemCrateOnLoadGuard
    {
        [HarmonyFinalizer]
        public static Exception Finalizer(Exception __exception) => null;
    }

    // 4. PRICE ENFORCEMENT
    [HarmonyPatch(typeof(ShipItem), "Awake")]
    public static class FurniturePriceAwakePatch
    {
        [HarmonyPostfix]
        public static void Postfix(ShipItem __instance) => ApplyEnforcedPrice(__instance);

        public static void ApplyEnforcedPrice(ShipItem item)
        {
            if (item == null) return;
            string name = item.gameObject.name.ToLower();

            if (name.StartsWith("cabinetsmall")) item.value = 480;
            else if (name.StartsWith("cabinetwide")) item.value = 720;
            else if (name.StartsWith("cabinet")) item.value = 1200;
            else if (name.Contains("chest") || name.Contains("seachest")) item.value = 800;
            else if (name.Contains("scroll") || name.Contains("shelf")) item.value = 450;
            else if (name.Contains("carpet")) item.value = 400;
            else if (name.Contains("navigatortable") || name.Contains("table")) item.value = 650;
            else if (name.Contains("bed")) item.value = 950;
        }
    }

    [HarmonyPatch(typeof(ShipItem), "Update")]
    public static class FurniturePriceUpdatePatch
    {
        [HarmonyPrefix]
        public static void Prefix(ShipItem __instance)
        {
            if (!__instance.sold)
            {
                FurniturePriceAwakePatch.ApplyEnforcedPrice(__instance);
            }
        }
    }

    // 5. GLOBAL PREFAB DIRECTORY INJECTION
    [HarmonyPatch(typeof(PrefabsDirectory), "PopulateShipItems")]
    public static class FurnitureDirectoryInjectionPatch
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            FurniturePlugin.ForceDirectDirectoryInjection();
        }
    }

    // 6. INTERCEPT LOOK UI PROMPTS
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

                if (extraTextField != null)
                {
                    string displayName = string.IsNullOrEmpty(component.lookText) ? component.name : component.lookText;
                    AccessTools.Property(extraTextField.GetType(), "text").SetValue(extraTextField, displayName);
                }

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

    // 7. INTERCEPT INTERACTION (ALT-ACTIVATE)
    [HarmonyPatch(typeof(ShipItem), nameof(ShipItem.OnAltActivate), new Type[0])]
    public static class ShipItemAltActivationPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ShipItem __instance)
        {
            // If unsold, hand off directly to vanilla purchase logic
            if (!__instance.sold) return true;

            var customLogic = __instance.GetComponent<ICustomFurnitureLogic>();
            return customLogic != null ? customLogic.OnAltActivate(__instance) : true;
        }
    }

    // 8. INTERCEPT RECTANGULAR STORAGE GRID DIMENSIONS
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