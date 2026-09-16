using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [System.Serializable]
    public class EquipmentSlot
    {
        public string slotName;
        public Item equippedItem;
        public Transform attachPoint;
    }

    [SerializeField] private EquipmentSlot[] equipmentSlots;
    private Inventory inventory;

    private void Start()
    {
        inventory = Inventory.instance;
    }

    public bool EquipItem(Item item)
    {
        if (item == null || !item.isEquippable)
        {
            Debug.LogWarning("Cannot equip this item");
            return false;
        }

        // Find appropriate slot (you can add slot type to Item if needed)
        foreach (EquipmentSlot slot in equipmentSlots)
        {
            if (slot.equippedItem == null)
            {
                slot.equippedItem = item;
                Debug.Log($"Equipped: {item.itemName}");
                return true;
            }
        }

        return false;
    }

    public bool UnequipItem(Item item)
    {
        foreach (EquipmentSlot slot in equipmentSlots)
        {
            if (slot.equippedItem == item)
            {
                slot.equippedItem = null;
                Debug.Log($"Unequipped: {item.itemName}");
                return true;
            }
        }

        return false;
    }

    public Item GetEquippedItem(string slotName)
    {
        foreach (EquipmentSlot slot in equipmentSlots)
        {
            if (slot.slotName == slotName)
            {
                return slot.equippedItem;
            }
        }

        return null;
    }

    public EquipmentSlot[] GetAllEquipment() => equipmentSlots;
}
