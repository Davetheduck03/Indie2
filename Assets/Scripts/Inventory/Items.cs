using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items
{
    public enum ItemType
    {
        FloorCleaner,
        PotatoChip,
        Grape,
        LightBulb,

    }
    public ItemType itemType;
    public int itemAmount;
}
