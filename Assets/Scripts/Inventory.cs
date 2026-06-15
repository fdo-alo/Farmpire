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

        public bool CanAddItem()
        {
            return count < maxAllowed;
        }

        public void AddItem(Item item)
        {
            itemName = item.data.itemName;
            icon = item.data.icon;
            count++;
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
            if (slot.itemName == item.data.itemName && slot.CanAddItem())
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

    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }
}