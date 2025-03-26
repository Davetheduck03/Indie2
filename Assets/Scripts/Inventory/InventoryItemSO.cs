using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class InventoryItemSO : ScriptableObject
    {
        public int itemID;
        public string itemName;
        public int maxStack = 5;
        public Sprite icon;
    }
