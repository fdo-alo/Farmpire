using UnityEngine;

public enum CropState
{
    Empty,
    Growing,
    Ready
}

[RequireComponent(typeof(SpriteRenderer))]
public class CropPlot : MonoBehaviour
{
    public Vector3Int CellPosition { get; private set; }
    public CropData CropData { get; private set; }
    public CropState State { get; private set; } = CropState.Empty;
    public bool IsWatered { get; private set; }

    private SpriteRenderer spriteRenderer;
    private int currentStageIndex;
    private float growthTimer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (State != CropState.Growing || CropData == null)
        {
            return;
        }

        growthTimer += Time.deltaTime;

        if (growthTimer >= CropData.secondsPerStage)
        {
            growthTimer = 0f;
            AdvanceStage();
        }
    }

    public void Plant(Vector3Int cellPosition, CropData cropData)
    {
        CellPosition = cellPosition;
        CropData = cropData;
        State = CropState.Growing;
        IsWatered = false;
        currentStageIndex = 0;
        growthTimer = 0f;

        RefreshSprite();
    }

    public void Water()
    {
        IsWatered = true;
    }

    public bool CanHarvest()
    {
        return State == CropState.Ready && CropData != null;
    }

    public ItemData Harvest()
    {
        if (!CanHarvest())
        {
            return null;
        }

        ItemData harvestItem = CropData.harvestItem;
        ResetPlot();
        return harvestItem;
    }

    private void AdvanceStage()
    {
        if (CropData.growthSprites == null || CropData.growthSprites.Length == 0)
        {
            State = CropState.Ready;
            return;
        }

        currentStageIndex++;

        if (currentStageIndex >= CropData.growthSprites.Length - 1)
        {
            currentStageIndex = CropData.growthSprites.Length - 1;
            State = CropState.Ready;
        }

        RefreshSprite();
    }

    private void RefreshSprite()
    {
        if (CropData == null || CropData.growthSprites == null || CropData.growthSprites.Length == 0)
        {
            spriteRenderer.sprite = null;
            return;
        }

        spriteRenderer.sprite = CropData.growthSprites[currentStageIndex];
    }

    private void ResetPlot()
    {
        CropData = null;
        State = CropState.Empty;
        IsWatered = false;
        currentStageIndex = 0;
        growthTimer = 0f;
        spriteRenderer.sprite = null;
    }
}
