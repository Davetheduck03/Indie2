// InventoryManager.cs
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform inventoryPanel;
    [SerializeField] private int inventorySize = 12;

    private List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        InitializeInventory();
    }

    void InitializeInventory()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slot = Instantiate(slotPrefab, inventoryPanel);
            slots.Add(slot.GetComponent<InventorySlot>());
        }
    }

    public bool AddItem(InventoryItemSO item, int count = 1)
    {
        // Check for existing stack
        foreach (InventorySlot slot in slots)
        {
            if (slot.HasItem && slot.Item == item && !slot.IsFull)
            {
                int spaceRemaining = item.maxStack - slot.StackCount;
                int addAmount = Mathf.Min(spaceRemaining, count);
                slot.AddToStack(addAmount);
                count -= addAmount;

                if (count <= 0) return true;
            }
        }

        // Find empty slot for remaining items
        foreach (InventorySlot slot in slots)
        {
            if (!slot.HasItem)
            {
                slot.SetItem(item, Mathf.Min(count, item.maxStack));
                count -= Mathf.Min(count, item.maxStack);

                if (count <= 0) return true;
            }
        }
        return false; // Inventory full
    }

    public void OnSlotClicked(InventorySlot slot)
    {
        if (!slot.HasItem) return;


        slot.Item.OnUseItem();

            slot.StackCount--;
            if (slot.StackCount <= 0)
            {
                slot.ClearSlot();
            }
            else
            {
                slot.UpdateUI();
            }
    }
}