using System;
using UnityEngine;

public class UpgradeableBuilding : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    public Sprite[] upgradeSprites;
    protected BoxCollider2D boxCollider;
    protected PolygonCollider2D polygonCollider;

    // Public variable to control debug logging from the Inspector
    public bool activateDebug = false; 

    protected virtual void Awake() // Using Awake for component initialization
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Ensure BoxCollider2D exists
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
            if (activateDebug) Debug.LogWarning($"[UpgradeableBuilding] Added missing BoxCollider2D to {gameObject.name}.");
        }
        else
        {
            if (activateDebug) Debug.Log($"[UpgradeableBuilding] Found BoxCollider2D on {gameObject.name}. Enabled state: {boxCollider.enabled}");
        }

        // Ensure PolygonCollider2D exists
        polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider == null)
        {
            polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
            if (activateDebug) Debug.LogWarning($"[UpgradeableBuilding] Added missing PolygonCollider2D to {gameObject.name}.");
        }
        else
        {
            if (activateDebug) Debug.Log($"[UpgradeableBuilding] Found PolygonCollider2D on {gameObject.name}. Enabled state: {polygonCollider.enabled}");
        }
    }

    protected virtual void Start() // Added for consistency, derived classes can override and call base.Start()
    {
        // Intentionally left empty for now. Derived classes can add their own Start logic.
    }

    // This method will be called to change the sprite and manage colliders
    public virtual void Upgrade(int upgradeLevel)
    {
        if (activateDebug) Debug.Log($"[UpgradeableBuilding] Upgrade called for {gameObject.name} with level: {upgradeLevel}");

        if (upgradeSprites != null && upgradeLevel >= 0 && upgradeLevel < upgradeSprites.Length)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = upgradeSprites[upgradeLevel];
            }
            else
            {
                if (activateDebug) Debug.LogWarning("SpriteRenderer not found on " + gameObject.name);
            }

            // Collider logic - colliders are guaranteed to exist by Awake()
            if (upgradeLevel == 0)
            {
                boxCollider.enabled = true;
                polygonCollider.enabled = false;
                if (activateDebug) Debug.Log($"[UpgradeableBuilding] {gameObject.name} - Level 0: BoxCollider enabled, PolygonCollider disabled.");
            }
            else if (upgradeLevel > 0)
            {
                boxCollider.enabled = false;
                polygonCollider.enabled = true;
                if (activateDebug) Debug.Log($"[UpgradeableBuilding] {gameObject.name} - Level > 0: BoxCollider disabled, PolygonCollider enabled.");
            }
            else
            {
                // This 'else' block would only be hit if upgradeLevel is negative,
                // which should be prevented by the initial check.
                boxCollider.enabled = false;
                polygonCollider.enabled = false;
                if (activateDebug) Debug.Log($"[UpgradeableBuilding] {gameObject.name} - Invalid Level: Both colliders disabled.");
            }
        }
        else
        {
            if (activateDebug) Debug.LogWarning($"[UpgradeableBuilding] Upgrade level out of bounds or upgradeSprites not set for {gameObject.name}: {upgradeLevel}");
        }
    }
}