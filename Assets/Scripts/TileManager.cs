using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile plowedTile;
    [SerializeField] private Tile wateredPlowedTile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null && tile.name == "Interactable_Visible")
            {
                interactableMap.SetTile(position, hiddenInteractableTile);
            }
        }
    }

    public bool IsInteractable(Vector3Int position)
    {
        return GetTileName(position) == "Interactable";
    }

    public bool IsPlowed(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);
        return tile != null && (tile == plowedTile || tile == wateredPlowedTile);
    }

    public bool IsWatered(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);
        return tile != null && tile == wateredPlowedTile;
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return interactableMap.WorldToCell(worldPosition);
    }

    public Vector3 GetCellCenterWorld(Vector3Int cellPosition)
    {
        return interactableMap.GetCellCenterWorld(cellPosition);
    }

    public void SetInteracted(Vector3Int position)
    {
        SetPlowed(position);
    }

    public void SetPlowed(Vector3Int position)
    {
        interactableMap.SetTile(position, plowedTile);
    }

    public void SetWatered(Vector3Int position)
    {
        if (wateredPlowedTile == null || !IsPlowed(position))
        {
            return;
        }

        interactableMap.SetTile(position, wateredPlowedTile);
    }

    public string GetTileName(Vector3Int position)
    {
        if (interactableMap != null)
        {
            TileBase tile = interactableMap.GetTile(position);

            if (tile != null)
            {
                return tile.name;
            }
        }

        return string.Empty;
    }
}
