using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform inventoryGrid;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private TextMeshProUGUI selectedItemName;
    [SerializeField] private TextMeshProUGUI selectedItemDescription;
    [SerializeField] private Image selectedItemIcon;
    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;
    
    private Inventory inventory;
    private InventorySlotUI[] slotUIs;
    private InventorySlotUI selectedSlot;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        inventory = Inventory.instance;
        canvasGroup = GetComponent<CanvasGroup>();
        
        if (inventory != null)
        {
            inventory.OnInventoryChanged += RefreshInventoryDisplay;
            InitializeSlots();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    private void InitializeSlots()
    {
        var slots = inventory.GetSlots();
        slotUIs = new InventorySlotUI[slots.Count];
        
        for (int i = 0; i < slots.Count; i++)
        {
            GameObject slotObject = Instantiate(inventorySlotPrefab, inventoryGrid);
            InventorySlotUI slotUI = slotObject.GetComponent<InventorySlotUI>();
            slotUI.Initialize(i, OnSlotSelected);
            slotUIs[i] = slotUI;
        }
    }

    private void RefreshInventoryDisplay()
    {
        var slots = inventory.GetSlots();
        
        for (int i = 0; i < slotUIs.Length && i < slots.Count; i++)
        {
            if (slots[i].IsEmpty())
            {
                slotUIs[i].Clear();
            }
            else
            {
                slotUIs[i].Display(slots[i].item, slots[i].quantity);
            }
        }
    }

    private void OnSlotSelected(InventorySlotUI slot)
    {
        selectedSlot = slot;
        
        if (slot.GetItem() != null)
        {
            selectedItemName.text = slot.GetItem().itemName;
            selectedItemDescription.text = slot.GetItem().description;
            selectedItemIcon.sprite = slot.GetItem().itemIcon;
            
            useButton.interactable = slot.GetItem().isConsumable;
            dropButton.interactable = true;
        }
        else
        {
            ClearSelectedDisplay();
        }
    }

    private void ClearSelectedDisplay()
    {
        selectedItemName.text = "";
        selectedItemDescription.text = "";
        selectedItemIcon.sprite = null;
        useButton.interactable = false;
        dropButton.interactable = false;
    }

    public void ToggleInventory()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0.5f ? 0f : 1f;
        canvasGroup.interactable = !canvasGroup.interactable;
        canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;
    }
}
