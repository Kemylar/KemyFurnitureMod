using System;
using UnityEngine;

namespace KemyFurniture
{
    public static class FurnitureSetup
    {
        public static void Configure(GameObject prefab, int prefabIndex, string name, int price, float mass)
        {
            if (prefab == null) return;

            // 1. Core Component Injection
            var saveComp = prefab.GetComponent<SaveablePrefab>() ?? prefab.AddComponent<SaveablePrefab>();
            saveComp.prefabIndex = prefabIndex;

            ShipItem shipItem = prefab.GetComponent<ShipItem>() ?? prefab.AddComponent<ShipItem>();
            shipItem.name = name;
            shipItem.value = price;
            shipItem.mass = mass;

            // Furniture rules matched to vanilla profile
            shipItem.big = true;
            shipItem.allowPlacingItems = true;

            // Core Nailing and Interaction Values
            shipItem.holdDistance = 1.15f;          // Determines raycast forward offset from player
            shipItem.furniturePlaceHeight = 0.15f;   // Matches vanilla table grounding tolerance
            shipItem.wallAttachment = false;        // Explicitly targets deck floor arrays

            // 2. Sync Rigidbody Mass
            Rigidbody rb = prefab.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.mass = mass;
            }

            // 3. Register child trigger tags
            foreach (Collider col in prefab.GetComponentsInChildren<Collider>(true))
            {
                if (col.gameObject != prefab)
                {
                    col.gameObject.tag = "ItemSubcollider";
                }
            }
        }
    }
}