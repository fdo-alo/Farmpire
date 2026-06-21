using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Inventory
{
    [Serializable]
    public class Slot
    {
        public string itemName;
        public int count;
        public int maxAllowed;
        public Sprite icon;

        public Slot()
        {
            itemName = "";
            count = 0;
            maxAllowed = 99;
        }

        public bool isEmpty => itemName == string.Empty && count == 0;

        public bool CanAddItem(string itemName)
        {
            return this.itemName == itemName && count < maxAllowed;
        }

        public void AddItem(Item item)
        {
            itemName = item.data.itemName;
            icon = item.data.icon;
            count++;
        }
        
        public void AddItem(string itemName, Sprite icon, int maxAllowed)
        {
            this.itemName = itemName;
            this.icon = icon;
            count++;
            this.maxAllowed = maxAllowed;
        }

        public void RemoveItem()
        {
            if (count > 0)
            {
                count--;
                if (count == 0)
                {
                    icon = null;
                    itemName = "";
                }
            }
        }
    }
    
    public List<Slot> slots = new List<Slot>();
    public Slot selectedSlot = null;

    public Inventory(int numSlots)
    {
        for (int i = 0; i < numSlots; i++)
        {
            Slot slot = new Slot();
            slots.Add(slot);
        }
    }

    public void Add(Item item)
    {
        // Try to add to an existing stack
        foreach (var slot in slots)
        {
            if (slot.itemName == item.data.itemName && slot.CanAddItem(item.data.itemName))
            {
                slot.AddItem(item);
                return; // Item added to existing stack, exit method
            }
        }
        
        // If not added to an existing stack, find an empty slot
        foreach (var slot in slots)
        {
            if (slot.itemName == string.Empty)
            {
                slot.AddItem(item);
                return; // Item added to a new slot, exit method
            }
        }

        // If no space was found (neither existing stack nor empty slot)
        Debug.LogWarning("Inventory is full! Could not add " + item.name);
    }

    public bool CanAdd(ItemData itemData, int amount = 1)
    {
        if (itemData == null || amount <= 0)
        {
            return false;
        }

        int remainingAmount = amount;

        foreach (var slot in slots)
        {
            if (slot.itemName == itemData.itemName)
            {
                remainingAmount -= slot.maxAllowed - slot.count;
            }
        }

        foreach (var slot in slots)
        {
            if (slot.isEmpty)
            {
                remainingAmount -= slot.maxAllowed;
            }
        }

        return remainingAmount <= 0;
    }

    public bool TryAdd(ItemData itemData, int amount = 1)
    {
        if (!CanAdd(itemData, amount))
        {
            return false;
        }

        for (int i = 0; i < amount; i++)
        {
            AddItemToFirstAvailableSlot(itemData);
        }

        return true;
    }

    private void AddItemToFirstAvailableSlot(ItemData itemData)
    {
        foreach (var slot in slots)
        {
            if (slot.itemName == itemData.itemName && slot.CanAddItem(itemData.itemName))
            {
                slot.AddItem(itemData.itemName, itemData.icon, slot.maxAllowed);
                return;
            }
        }

        foreach (var slot in slots)
        {
            if (slot.isEmpty)
            {
                slot.AddItem(itemData.itemName, itemData.icon, slot.maxAllowed);
                return;
            }
        }
    }

    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }

    public void Remove(int index, int numToRemove)
    {
        if(slots[index].count >= numToRemove)
        {
            for(int i = 0; i < numToRemove; i++)
            {
                Remove(index);
            }
        }
    }

    public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory, int numToMove=1)
    {
        Slot fromSlot = slots[fromIndex];
        Slot toSlot= toInventory.slots[toIndex];

        if (toSlot.isEmpty || toSlot.CanAddItem(fromSlot.itemName))
        {
            for (int i = 0; i < numToMove; i++)
            {
                toSlot.AddItem(fromSlot.itemName, fromSlot.icon, fromSlot.maxAllowed);
                fromSlot.RemoveItem();
            }
        }
    }

    public void SelectSlot(int index)
    {
        if (slots != null && slots.Count > 0)
        {
            selectedSlot = slots[index];
        }
    }
}
