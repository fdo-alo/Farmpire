using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
   public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();
   
   public GameObject inventoryPanel;
   
   public List<Inventory_UI> inventoryUis;

   public static Slot_UI draggedSlot;
   public static Image draggedIcon;
   public static bool dragSingle;

   public bool IsInventoryOpen => inventoryPanel != null && inventoryPanel.activeSelf;

   private void Awake()
   {
      Initialize();
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.B))
      {
         ToggleInventoryUI();
      }
      
      dragSingle = Input.GetKey(KeyCode.LeftShift);
   }
   

   public void ToggleInventoryUI()
   {
      if (inventoryPanel != null)
      {
         if(!inventoryPanel.activeSelf)
         {
            inventoryPanel.SetActive(true);
            RefreshInventoryUI("Backpack");
         }
         else
         {
            inventoryPanel.SetActive(false);
         }
      }
   }

   public void RefreshInventoryUI(string inventoryName)
   {
      if (inventoryUIByName.ContainsKey(inventoryName))
      {
         inventoryUIByName[inventoryName].Refresh();
      }
   }

   public void RefreshAll()
   {
      foreach (var keyValuePair in inventoryUIByName)
      {
         keyValuePair.Value.Refresh();
      }
   }

   public Inventory_UI GetInventoryUI(string inventoryName)
   {
      if (inventoryUIByName.ContainsKey(inventoryName))
      {
         return inventoryUIByName[inventoryName];
      }
      Debug.LogWarning("There is not inventory ui for " + inventoryName);
      return  null;
   }
   
   private void Initialize()
   {
      foreach (var ui in inventoryUis)
      {
         inventoryUIByName.TryAdd(ui.inventoryName, ui);
      }
   }
}
