using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileSelector : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Sprite validSprite;
    [SerializeField] private Sprite interactSprite;
    [SerializeField] private Sprite invalidSprite;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
        }
    }

    private void Update()
    {
        if (IsInventoryOpen() || player == null || !player.TryGetTargetCell(out Vector3Int targetCell, out Vector3 targetCellCenter))
        {
            SetVisible(false);
            return;
        }

        transform.position = targetCellCenter;

        bool isValidTarget = player.CanUseSelectedToolAt(targetCell, targetCellCenter);
        bool isInteractTarget = player.CanInteractAt(targetCell, targetCellCenter);

        if (isValidTarget)
        {
            spriteRenderer.sprite = validSprite;
        }
        else if (isInteractTarget)
        {
            spriteRenderer.sprite = interactSprite;
        }
        else
        {
            spriteRenderer.sprite = invalidSprite;
        }

        SetVisible(spriteRenderer.sprite != null);
    }

    private void SetVisible(bool isVisible)
    {
        if (spriteRenderer.enabled != isVisible)
        {
            spriteRenderer.enabled = isVisible;
        }
    }

    private bool IsInventoryOpen()
    {
        return GameManager.instance != null
            && GameManager.instance.uiManager != null
            && GameManager.instance.uiManager.IsInventoryOpen;
    }
}
