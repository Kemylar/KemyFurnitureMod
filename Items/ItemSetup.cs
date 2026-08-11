using UnityEngine;
using KemyFurniture.Items.ScrollShelf;
using KemyFurniture.Items.Cabinet;
using KemyFurniture.Items.SeaChest;
using KemyFurniture.Items.Bed;
using KemyFurniture.Items.Carpet;
using KemyFurniture.Items.NavigatorDesk;

namespace KemyFurniture.Core
{
    public static class ItemSetup
    {
        public static void RegisterSaveIndex(GameObject prefab, int fallbackIndex)
        {
            if (prefab == null) return;

            SaveablePrefab saveComp = prefab.GetComponent<SaveablePrefab>();
            if (saveComp != null)
            {
                if (saveComp.prefabIndex == 0) saveComp.prefabIndex = fallbackIndex;
            }
            else
            {
                FurniturePlugin.DiagLogger.LogError($"[KEMY FURNITURE] Missing SaveablePrefab on {prefab.name}!");
            }

            // Dynamically ensure logic components are present
            EnsureLogicComponents(prefab);
        }

        private static void EnsureLogicComponents(GameObject prefab)
        {
            string name = prefab.name.ToLower();

            if (name.Contains("shelf") && prefab.GetComponent<ScrollShelfLogic>() == null)
            {
                prefab.AddComponent<ScrollShelfLogic>();
            }
            else if (name.Contains("cabinet") && prefab.GetComponent<CabinetLogic>() == null)
            {
                prefab.AddComponent<CabinetLogic>();
            }
            else if (name.Contains("chest") && prefab.GetComponent<SeaChestLogic>() == null)
            {
                prefab.AddComponent<SeaChestLogic>();
            }
            else if (name.Contains("bed") && prefab.GetComponent<BedLogic>() == null)
            {
                prefab.AddComponent<BedLogic>();
            }
            else if (name.Contains("carpet") && prefab.GetComponent<CarpetLogic>() == null)
            {
                prefab.AddComponent<CarpetLogic>();
            }
            else if ((name.Contains("desk") || name.Contains("table") || name.Contains("navigator")) && prefab.GetComponent<NavigatorDeskLogic>() == null)
            {
                prefab.AddComponent<NavigatorDeskLogic>();
            }
        }
    }
}