using System;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();

        if (player)
        {
            Item item = GetComponent<Item>();
            if (item != null)
            {
                player.inventory.Add("Backpack", item);
                Destroy(gameObject);
            }
        }
    }
}