using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileSelector : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Sprite validSprite;
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
        if (player == null || !player.TryGetTargetCell(out Vector3Int targetCell, out Vector3 targetCellCenter))
        {
            SetVisible(false);
            return;
        }

        transform.position = targetCellCenter;

        bool isValidTarget = player.CanUseSelectedToolAt(targetCell, targetCellCenter);
        spriteRenderer.sprite = isValidTarget ? validSprite : invalidSprite;
        SetVisible(spriteRenderer.sprite != null);
    }

    private void SetVisible(bool isVisible)
    {
        if (spriteRenderer.enabled != isVisible)
        {
            spriteRenderer.enabled = isVisible;
        }
    }
}
