using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory instance { get; private set; }
    
    [SerializeField] private int maxSlots = 12;
    [SerializeField] private float maxWeight = 50f;
    private List<InventorySlot> slots;
    private float currentWeight = 0f;
    
    public delegate void InventoryChangeDelegate();
    public event InventoryChangeDelegate OnInventoryChanged;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeInventory();
    }

    private void InitializeInventory()
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < maxSlots; i++)
        {
            slots.Add(new InventorySlot());
        }
        Debug.Log($"Inventory initialized with {maxSlots} slots");
    }

    public bool AddItem(Item item, int quantity = 1)
    {
        if (item == null || quantity <= 0)
        {
            Debug.LogWarning("Invalid item or quantity");
            return false;
        }

        float itemWeight = item.weight * quantity;
        if (currentWeight + itemWeight > maxWeight)
        {
            Debug.LogWarning($"Inventory full! Cannot add {item.itemName}");
            return false;
        }

        // Try to stack with existing items
        if (item.maxStackSize > 1)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.item == item && slot.quantity < item.maxStackSize)
                {
                    int spaceLeft = item.maxStackSize - slot.quantity;
                    int toAdd = Mathf.Min(quantity, spaceLeft);
                    slot.quantity += toAdd;
                    currentWeight += item.weight * toAdd;
                    quantity -= toAdd;
                    
                    if (quantity == 0)
                    {
                        OnInventoryChanged?.Invoke();
                        Debug.Log($"Added {toAdd} x {item.itemName}");
                        return true;
                    }
                }
            }
        }

        // Find empty slot
        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                int toAdd = Mathf.Min(quantity, item.maxStackSize);
                slot.item = item;
                slot.quantity = toAdd;
                currentWeight += item.weight * toAdd;
                quantity -= toAdd;
                
                if (quantity == 0)
                {
                    OnInventoryChanged?.Invoke();
                    Debug.Log($"Added {toAdd} x {item.itemName}");
                    return true;
                }
            }
        }

        Debug.LogWarning($"Could not add all {item.itemName}. Inventory full");
        return false;
    }

    public bool RemoveItem(Item item, int quantity = 1)
    {
        if (item == null || quantity <= 0)
            return false;

        int toRemove = quantity;
        
        for (int i = slots.Count - 1; i >= 0; i--)
        {
            if (slots[i].item == item)
            {
                int removed = Mathf.Min(toRemove, slots[i].quantity);
                slots[i].quantity -= removed;
                currentWeight -= item.weight * removed;
                toRemove -= removed;
                
                if (slots[i].quantity <= 0)
                {
                    slots[i].Clear();
                }
                
                if (toRemove == 0)
                {
                    OnInventoryChanged?.Invoke();
                    Debug.Log($"Removed {quantity} x {item.itemName}");
                    return true;
                }
            }
        }

        return false;
    }

    public bool HasItem(Item item, int quantity = 1)
    {
        int count = 0;
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item)
            {
                count += slot.quantity;
            }
        }
        return count >= quantity;
    }

    public int GetItemCount(Item item)
    {
        int count = 0;
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item)
            {
                count += slot.quantity;
            }
        }
        return count;
    }

    public void ClearInventory()
    {
        foreach (InventorySlot slot in slots)
        {
            slot.Clear();
        }
        currentWeight = 0f;
        OnInventoryChanged?.Invoke();
        Debug.Log("Inventory cleared");
    }

    public List<InventorySlot> GetSlots() => slots;
    public float GetCurrentWeight() => currentWeight;
    public float GetMaxWeight() => maxWeight;
    public float GetWeightPercentage() => (currentWeight / maxWeight) * 100f;
    public int GetEmptySlotCount() => slots.FindAll(s => s.IsEmpty()).Count;
    public bool IsInventoryFull() => GetEmptySlotCount() == 0;
}
