using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public string inventoryName;
    public List<Slot_UI> slots = new List<Slot_UI>();

    [SerializeField] private Canvas canvas;
    
    private bool dragSingle;

    private Inventory inventory;

    private void Awake()
    {
        canvas = FindAnyObjectByType<Canvas>();
    }

    private void Start()
    {
        inventory = GameManager.instance.player.inventory.GetInventoryByName(inventoryName);
        SetupSlots();
        Refresh();
    }

    public void Refresh()
    {
        if (slots.Count == inventory.slots.Count)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (inventory.slots[i].itemName != string.Empty)
                {
                    slots[i].SetItem(inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    public void Remove()
    {
        var itemToDrop = GameManager.instance.itemManager.GetItemByName(inventory.slots[UI_Manager.draggedSlot.slotID].itemName);
        if (itemToDrop != null)
        {
            if (UI_Manager.dragSingle)
            {
                GameManager.instance.player.DropItem(itemToDrop);
                inventory.Remove(UI_Manager.draggedSlot.slotID);
            }
            else
            {
                GameManager.instance.player.DropItem(itemToDrop, inventory.slots[UI_Manager.draggedSlot.slotID].count);
                inventory.Remove(UI_Manager.draggedSlot.slotID, inventory.slots[UI_Manager.draggedSlot.slotID].count);
            }

            Refresh();
        }

        UI_Manager.draggedSlot = null;
    }

    public void SlotBeginDrag(Slot_UI slot)
    {
        if (slot == null || slot.inventory == null || slot.inventory.slots[slot.slotID].isEmpty)
        {
            return;
        }

        UI_Manager.draggedSlot = slot;

        // Instantiate a copy of the itemIcon for dragging
        UI_Manager.draggedIcon = Instantiate(UI_Manager.draggedSlot.itemIcon, canvas.transform);
        UI_Manager.draggedIcon.raycastTarget = false; // Make sure it doesn't block raycasts

        // Do NOT set a fixed size here. It should inherit the original size or be scaled as desired.
        UI_Manager.draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);

        // Hide the original item icon in the slot
        //draggedSlot.itemIcon.color = new Color(1, 1, 1, 0);

        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
        //Debug.Log("Start Drag: " + draggedSlot.name);
    }

    public void SlotDrag()
    {
        if (UI_Manager.draggedIcon == null)
        {
            return;
        }

        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
    }

    public void SlotEndDrag()
    {
        //Debug.Log("Done Drag: " + draggedSlot.name);
        // Destroy the instantiated dragged icon
        if (UI_Manager.draggedIcon != null)
        {
            Destroy(UI_Manager.draggedIcon.gameObject);
        }

        UI_Manager.draggedIcon = null;
    }

    public void SlotDrop(Slot_UI slot)
    {
        if (slot == null || slot.inventory == null || UI_Manager.draggedSlot == null || UI_Manager.draggedSlot.inventory == null)
        {
            return;
        }
        
        if (UI_Manager.dragSingle)
        {
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory);
            
        }
        else
        {
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory,
                UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID].count);
        }
        GameManager.instance.uiManager.RefreshAll();
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if (canvas != null)
        {
            Vector2 mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, Input.mousePosition,
                null, out mousePosition);

            toMove.transform.position = canvas.transform.TransformPoint(mousePosition);
        }
    }

    private void SetupSlots()
    {
        int counter = 0;

        foreach (var slot in slots)
        {
            slot.Setup(counter, inventory, this);
            counter++;
        }
    }
}
