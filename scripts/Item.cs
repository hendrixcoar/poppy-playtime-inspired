using UnityEngine;
using System.Collections.Generic;

public class Item : ScriptableObject
{
    public string itemID;
    public string itemName;
    [TextArea(3, 5)]
    public string description;
    public Sprite itemIcon;
    public int maxStackSize = 1;
    public float weight = 0.5f;
    public bool isConsumable = false;
    public bool isEquippable = false;
    public ItemRarity rarity = ItemRarity.Common;
    
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}

public class InventorySlot
{
    public Item item;
    public int quantity = 0;
    
    public InventorySlot()
    {
        item = null;
        quantity = 0;
    }
    
    public InventorySlot(Item newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
    }
    
    public bool IsEmpty() => item == null || quantity <= 0;
    
    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}
