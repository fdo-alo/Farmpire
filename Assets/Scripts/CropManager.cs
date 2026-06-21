using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{
    [SerializeField] private CropPlot cropPlotPrefab;

    private readonly Dictionary<Vector3Int, CropPlot> cropsByCell = new Dictionary<Vector3Int, CropPlot>();

    public bool HasCrop(Vector3Int cellPosition)
    {
        return cropsByCell.ContainsKey(cellPosition);
    }

    public bool TryPlant(Vector3Int cellPosition, Vector3 worldPosition, CropData cropData)
    {
        if (cropPlotPrefab == null || cropData == null || HasCrop(cellPosition))
        {
            return false;
        }

        CropPlot cropPlot = Instantiate(cropPlotPrefab, worldPosition, Quaternion.identity, transform);
        cropPlot.Plant(cellPosition, cropData);
        cropsByCell.Add(cellPosition, cropPlot);
        return true;
    }

    public bool TryWater(Vector3Int cellPosition)
    {
        if (!cropsByCell.TryGetValue(cellPosition, out CropPlot cropPlot))
        {
            return false;
        }

        cropPlot.Water();
        return true;
    }

    public ItemData TryHarvest(Vector3Int cellPosition)
    {
        if (!cropsByCell.TryGetValue(cellPosition, out CropPlot cropPlot) || !cropPlot.CanHarvest())
        {
            return null;
        }

        ItemData harvestItem = cropPlot.Harvest();
        cropsByCell.Remove(cellPosition);
        Destroy(cropPlot.gameObject);
        return harvestItem;
    }
}
