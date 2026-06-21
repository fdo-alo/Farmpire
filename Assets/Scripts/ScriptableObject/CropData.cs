using UnityEngine;

[CreateAssetMenu(fileName = "Crop Data", menuName = "Crop Data", order = 51)]
public class CropData : ScriptableObject
{
    public string cropName = "Crop Name";
    public ItemData seedItem;
    public ItemData harvestItem;
    public Sprite[] growthSprites;
    public float secondsPerStage = 5f;
    public int harvestAmount = 1;
    public int xpReward = 1;
}
