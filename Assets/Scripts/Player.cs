using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
   public InventoryManager inventoryManager;
   [SerializeField] private float toolRange = 2.5f;
   [SerializeField] private CropManager cropManager;
   [SerializeField] private CropData testCropData;

   private TileManager tileManager;
   private Vector2Int facingDirection = Vector2Int.down;

   private void Awake()
   {
      inventoryManager = GetComponent<InventoryManager>();
   }

   private void Start()
   {
      tileManager = GameManager.instance.tileManager;

      if (cropManager == null)
      {
         cropManager = FindAnyObjectByType<CropManager>();
      }
   }

   private void Update()
   {
      UpdateFacingDirection();

      if (Input.GetMouseButtonDown(0))
      {
         UseSelectedToolAtMouse();
      }
   }

   private void UseSelectedToolAtMouse()
   {
      if (!TryGetTargetCell(out Vector3Int targetCell, out Vector3 targetCellCenter))
      {
         return;
      }

      FaceTarget(targetCellCenter);

      if (HasSelectedTool("Hoe"))
      {
         UseHoe(targetCell);
         return;
      }

      if (HasSelectedSeed(testCropData))
      {
         PlantCrop(targetCell, targetCellCenter, testCropData);
      }
   }

   public bool TryGetTargetCell(out Vector3Int targetCell, out Vector3 targetCellCenter)
   {
      targetCell = default;
      targetCellCenter = default;

      if (tileManager == null || Camera.main == null)
      {
         return false;
      }

      if (!TryGetMouseWorldPosition(out Vector3 mouseWorldPosition))
      {
         return false;
      }

      targetCell = tileManager.WorldToCell(mouseWorldPosition);
      targetCellCenter = tileManager.GetCellCenterWorld(targetCell);
      return true;
   }

   public bool IsTargetCellInToolRange(Vector3 targetCellCenter)
   {
      return Vector2.Distance(transform.position, targetCellCenter) <= toolRange;
   }

   public bool CanUseSelectedToolAt(Vector3Int targetCell, Vector3 targetCellCenter)
   {
      if (!IsTargetCellInToolRange(targetCellCenter))
      {
         return false;
      }

      if (HasSelectedTool("Hoe"))
      {
         return tileManager != null && tileManager.IsInteractable(targetCell);
      }

      if (HasSelectedSeed(testCropData))
      {
         return cropManager != null
            && testCropData != null
            && tileManager != null
            && tileManager.IsPlowed(targetCell)
            && !cropManager.HasCrop(targetCell);
      }

      return false;
   }

   private void UseHoe(Vector3Int targetCell)
   {
      if (!tileManager.IsInteractable(targetCell))
      {
         return;
      }

      tileManager.SetInteracted(targetCell);
   }

   private void PlantCrop(Vector3Int targetCell, Vector3 targetCellCenter, CropData cropData)
   {
      if (cropManager == null || cropData == null || !tileManager.IsPlowed(targetCell))
      {
         return;
      }

      cropManager.TryPlant(targetCell, targetCellCenter, cropData);
   }

   public bool HasSelectedTool(string itemName)
   {
      Inventory.Slot selectedSlot = inventoryManager.toolbar.selectedSlot;
      return selectedSlot != null && selectedSlot.itemName == itemName;
   }

   public bool HasSelectedSeed(CropData cropData)
   {
      if (cropData == null || cropData.seedItem == null)
      {
         return false;
      }

      return HasSelectedTool(cropData.seedItem.itemName);
   }

   private bool TryGetMouseWorldPosition(out Vector3 mouseWorldPosition)
   {
      Plane gameplayPlane = new Plane(Vector3.forward, Vector3.zero);
      Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

      if (gameplayPlane.Raycast(mouseRay, out float distance))
      {
         mouseWorldPosition = mouseRay.GetPoint(distance);
         mouseWorldPosition.z = 0;
         return true;
      }

      mouseWorldPosition = Vector3.zero;
      return false;
   }

   private void FaceTarget(Vector3 targetPosition)
   {
      Vector2 directionToTarget = targetPosition - transform.position;

      if (Mathf.Abs(directionToTarget.x) > Mathf.Abs(directionToTarget.y))
      {
         facingDirection = directionToTarget.x > 0 ? Vector2Int.right : Vector2Int.left;
      }
      else
      {
         facingDirection = directionToTarget.y > 0 ? Vector2Int.up : Vector2Int.down;
      }
   }

   private void UpdateFacingDirection()
   {
      float horizontal = Input.GetAxisRaw("Horizontal");
      float vertical = Input.GetAxisRaw("Vertical");

      if (Mathf.Approximately(horizontal, 0) && Mathf.Approximately(vertical, 0))
      {
         return;
      }

      if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
      {
         facingDirection = horizontal > 0 ? Vector2Int.right : Vector2Int.left;
      }
      else
      {
         facingDirection = vertical > 0 ? Vector2Int.up : Vector2Int.down;
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
