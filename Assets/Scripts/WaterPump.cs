using UnityEngine;

public enum WaterPumpState
{
    Pond,
    Pump
}

public class WaterPump : UpgradeableBuilding
{
    public WaterPumpState waterPumpState = WaterPumpState.Pond;

    [Header("Pump Animation and Fill Settings")]
    public float fillDuration = 5f; // Time in seconds to fill the pump
    public bool isFull; // Public for inspection, but managed internally

    private Animator animator;
    private float currentFillTime;

    protected override void Awake()
    {
        base.Awake(); // Call the base class Awake to initialize spriteRenderer, colliders
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start(); // Call base Start if it had any logic

        // Always call Upgrade with the initial state to configure sprites and colliders.
        // The validation of upgradeSprites is done within the Upgrade method.
        Upgrade((int)waterPumpState);
    }

    void Update()
    {
        if (waterPumpState == WaterPumpState.Pump)
        {
            if (!isFull)
            {
                currentFillTime += Time.deltaTime;
                if (currentFillTime >= fillDuration)
                {
                    isFull = true;
                    currentFillTime = fillDuration; // Cap the time
                }
            }

            if (animator != null)
            {
                animator.SetBool("isFull", isFull);
            }
        }
    }

    public override void Upgrade(int upgradeLevel)
    {
        base.Upgrade(upgradeLevel); // Call the base class's upgrade logic
        waterPumpState = (WaterPumpState)upgradeLevel; // Update the waterPumpState enum

        if (animator != null)
        {
            if (waterPumpState == WaterPumpState.Pump)
            {
                animator.enabled = true;
                // Reset fill state when upgrading to Pump state
                isFull = false;
                currentFillTime = 0f;
                animator.SetBool("isFull", isFull); // Ensure animator reflects initial state
            }
            else // WaterPumpState.Pond or other states without animation
            {
                animator.enabled = false;
                isFull = false; // Pond is never "full" in the pump sense
                currentFillTime = 0f;
            }
        }
    }

    // Call this method when the player collects water
    public void CollectWater()
    {
        if (waterPumpState == WaterPumpState.Pump && isFull)
        {
            isFull = false;
            currentFillTime = 0f;
            if (animator != null)
            {
                animator.SetBool("isFull", isFull); // Update animator immediately
            }
            Debug.Log("Water collected from the pump!");
            // Add logic here for giving water to the player's inventory
        }
        else if (waterPumpState == WaterPumpState.Pond)
        {
            Debug.Log("Cannot collect water from a pond in this manner.");
        }
        else
        {
            Debug.Log("Water pump is not full yet.");
        }
    }
}