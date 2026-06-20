using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot_UI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    public int slotID;
    public Inventory inventory;
    public Image itemIcon;
    public TextMeshProUGUI quantityText;

    [SerializeField] private GameObject highlight;

    private Inventory_UI inventoryUI;
    private Toolbar_UI toolbarUI;

    private void Awake()
    {
        toolbarUI = GetComponentInParent<Toolbar_UI>();

        foreach (var eventTrigger in GetComponents<EventTrigger>())
        {
            eventTrigger.enabled = false;
        }
    }

    public void Setup(int id, Inventory sourceInventory, Inventory_UI sourceInventoryUI)
    {
        slotID = id;
        inventory = sourceInventory;
        inventoryUI = sourceInventoryUI;
    }

    public void SetItem(Inventory.Slot slot)
    {
        if (slot != null)
        {
            itemIcon.sprite = slot.icon;
            itemIcon.color = new Color(1, 1, 1, 1);
            quantityText.text = slot.count.ToString();
        }
    }

    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }

    public void SetHighlight(bool isOn)
    {
        highlight.SetActive(isOn);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (inventoryUI == null || inventory == null || inventory.slots[slotID].isEmpty)
        {
            return;
        }

        inventoryUI.SlotBeginDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        inventoryUI?.SlotDrag();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        inventoryUI?.SlotEndDrag();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (inventoryUI == null || UI_Manager.draggedSlot == null)
        {
            return;
        }

        inventoryUI.SlotDrop(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        toolbarUI?.SelectSlot(slotID);
    }
}
