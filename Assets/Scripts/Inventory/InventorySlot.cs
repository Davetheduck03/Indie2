using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public Text countText;

    public InventoryItemSO Item { get; set; }
    public int StackCount { get; set; }
    public bool HasItem => Item != null;
    public bool IsFull => HasItem && StackCount >= Item.maxStack;

    public void SetItem(InventoryItemSO item, int count)
    {
        Item = item;
        StackCount = count;
        UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryManager.Instance.OnSlotClicked(this);
        }
    }

    public void AddToStack(int amount)
    {
        StackCount += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (HasItem)
        {
            iconImage.sprite = Item.icon;
            iconImage.enabled = true;
            countText.text = StackCount > 1 ? StackCount.ToString() : "";
            countText.gameObject.SetActive(StackCount > 1);
        }
        else ClearSlot();
    }

    public void ClearSlot()
    {
        Item = null;
        StackCount = 0;
        iconImage.enabled = false;
        countText.gameObject.SetActive(false);
    }
}