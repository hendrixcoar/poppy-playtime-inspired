using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Image selectionHighlight;
    [SerializeField] private Button slotButton;
    
    private Item item;
    private int quantity;
    private int slotIndex;
    private System.Action<InventorySlotUI> onSelected;

    public void Initialize(int index, System.Action<InventorySlotUI> callback)
    {
        slotIndex = index;
        onSelected = callback;
        slotButton.onClick.AddListener(OnSlotClicked);
        Clear();
    }

    public void Display(Item newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
        
        if (itemIcon != null)
            itemIcon.sprite = newItem.itemIcon;
        
        if (quantityText != null)
            quantityText.text = newQuantity > 1 ? newQuantity.ToString() : "";
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
        
        if (itemIcon != null)
            itemIcon.sprite = null;
        
        if (quantityText != null)
            quantityText.text = "";
    }

    private void OnSlotClicked()
    {
        onSelected?.Invoke(this);
    }

    public void Select(bool isSelected)
    {
        if (selectionHighlight != null)
            selectionHighlight.enabled = isSelected;
    }

    public Item GetItem() => item;
    public int GetQuantity() => quantity;
    public int GetSlotIndex() => slotIndex;
}
