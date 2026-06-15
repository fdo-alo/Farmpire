using UnityEngine;

public enum HouseState
{
    Destroyed,
    SimpleHouse
}

public class House : UpgradeableBuilding // Inherit from UpgradeableBuilding
{
    public HouseState houseState = HouseState.Destroyed;
    
    protected override void Start()
    {
        base.Start(); // Calls the base class's Start (if it has logic)
        // The Awake logic in UpgradeableBuilding already ensures colliders exist.

        // Always call Upgrade with the initial state to configure sprites and colliders.
        // The validation of upgradeSprites is done within the Upgrade method.
        Upgrade((int)houseState);
    }

    void Update()
    {
        
    }

    public override void Upgrade(int upgradeLevel)
    {
        base.Upgrade(upgradeLevel); // Calls the base class's upgrade logic
        // Add any House-specific upgrade logic here if needed
        houseState = (HouseState)upgradeLevel; // Updates the houseState enum
    }
}