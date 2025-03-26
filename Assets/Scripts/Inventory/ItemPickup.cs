using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public InventoryItemSO itemData;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
            if (InventoryManager.Instance.AddItem(itemData, amount))
            {
                Destroy(gameObject);
            }
    }
}