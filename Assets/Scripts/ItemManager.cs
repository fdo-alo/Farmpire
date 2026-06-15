using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] items;

    private Dictionary<string, Item> nameToItemDict =
        new Dictionary<string, Item>();

    private void Awake()
    {
        foreach (var item in items)
        {
            AddItem(item);
        }
    }

    private void AddItem(Item item)
    {
        nameToItemDict.TryAdd(item.data.itemName, item);
    }

    public Item GetItemByName(string type)
    {
        return nameToItemDict.GetValueOrDefault(type);
    }
}
