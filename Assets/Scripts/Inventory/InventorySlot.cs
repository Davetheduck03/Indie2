using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Text countText;

    public InventoryItemSO Item { get; private set; }
    public int StackCount { get; private set; }
    public bool HasItem => Item != null;
    public bool IsFull => HasItem && StackCount >= Item.maxStack;

    public void SetItem(InventoryItemSO item, int count)
    {
        Item = item;
        StackCount = count;
        UpdateUI();
    }

    public void AddToStack(int amount)
    {
        StackCount += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        iconImage.sprite = Item.icon;
        iconImage.enabled = true;
        countText.text = StackCount > 1 ? StackCount.ToString() : "";
        countText.gameObject.SetActive(StackCount > 1);
    }

    public void ClearSlot()
    {
        Item = null;
        StackCount = 0;
        iconImage.enabled = false;
        countText.gameObject.SetActive(false);
    }
}