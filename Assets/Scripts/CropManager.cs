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

    public bool HasReadyCrop(Vector3Int cellPosition)
    {
        return cropsByCell.TryGetValue(cellPosition, out CropPlot cropPlot) && cropPlot.CanHarvest();
    }

    public bool TryPlant(Vector3Int cellPosition, Vector3 worldPosition, CropData cropData, bool isWatered = false)
    {
        if (cropPlotPrefab == null || cropData == null || HasCrop(cellPosition))
        {
            return false;
        }

        CropPlot cropPlot = Instantiate(cropPlotPrefab, worldPosition, Quaternion.identity, transform);
        cropPlot.Plant(cellPosition, cropData, isWatered);
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

    public bool TryGetHarvest(Vector3Int cellPosition, out ItemData harvestItem, out int harvestAmount)
    {
        harvestItem = null;
        harvestAmount = 0;

        if (!cropsByCell.TryGetValue(cellPosition, out CropPlot cropPlot) || !cropPlot.CanHarvest())
        {
            return false;
        }

        harvestItem = cropPlot.CropData.harvestItem;
        harvestAmount = Mathf.Max(1, cropPlot.CropData.harvestAmount);
        return harvestItem != null;
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
