using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ItemPickup.cs
public class ItemPickup : MonoBehaviour
{
    public InventoryItemSO itemData;
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (InventoryManager.Instance.AddItem(itemData, amount))
            {
                Destroy(gameObject);
            }
        }
    }
}