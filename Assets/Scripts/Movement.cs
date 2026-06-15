using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Animator animator;
    
    public float speed;
    public float runSpeedMultiplier = 1.5f; // How much faster the player runs (e.g., 1.5x normal speed)

    private Vector3 direction;
    private float currentSpeed; // To store the actual speed being used

    private void Start()
    {
        currentSpeed = speed; // Initialize currentSpeed with the base speed
    }

    private void Update()
    {
        float  horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        direction = new Vector3(horizontal, vertical).normalized;
        
        // Check for run input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = speed * runSpeedMultiplier;
        }
        else
        {
            currentSpeed = speed;
        }

        AnimateMovement(direction);
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * (currentSpeed * Time.deltaTime));
    }

    private void AnimateMovement(Vector3 direction)
    {
        if(animator != null)
        {
            if (direction.magnitude > 0)
            {
                animator.SetBool("isMoving", true);
                
                animator.SetFloat("horizontal", direction.x);
                animator.SetFloat("vertical", direction.y);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }
}