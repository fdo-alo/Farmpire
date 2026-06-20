using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
   public InventoryManager inventory;

   private void Awake()
   {
      inventory =GetComponent<InventoryManager>();
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Space))
      {
         Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

         if (GameManager.instance.tileManager.IsInteractable(position))
         {
            Debug.Log("Tile is interactable");
            GameManager.instance.tileManager.SetInteracted(position);
         }
         else
         {
            Debug.Log("Tile is not interactable");
         }
      }
   }

   public void DropItem(Item item)
   {
      Vector2 spawnLocation = transform.position;

      Vector2 spawnOffset = Random.insideUnitCircle * 1.25f;
      
      Item droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);
      
      droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
   }
   
   public void DropItem(Item item, int numToDrop)
   {
      for (int i = 0; i < numToDrop; i++)
      {
         DropItem(item);
      }
   }
}
