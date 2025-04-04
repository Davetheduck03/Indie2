using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { HungerConsumable, HealthConsumable, Weapon, Buff }

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItemSO : ScriptableObject 
{
    public int itemID;
    public string itemName;
    public int maxStack;
    public Sprite icon;
    public ItemType itemType;
    public int healthRestoreAmount;
    public int hungerRestoreAmount;
    public float buffTime;
    public float speedModifier;


    public void OnUseItem()
    {
        switch (itemType)
        {
            case ItemType.HealthConsumable:
                Debug.Log($"Used {itemName}");
                Player.Instance.AddHealth(healthRestoreAmount);
                break;

            case ItemType.Weapon:
                Debug.Log($"Equipped {itemName}");
                break;

            case ItemType.HungerConsumable:
                Debug.Log($"Used {itemName}");
                Player.Instance.AddPlayerHunger(hungerRestoreAmount);
                break;
            case ItemType.Buff when speedModifier > 1:
                Player.Instance.SpeedModifier(speedModifier, buffTime);
                break;
            default:
                Debug.Log("Item Type not existed");
                break;
        }
    }
}