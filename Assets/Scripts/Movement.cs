using UnityEngine;

public class Movement : MonoBehaviour
{
    public Animator animator;
    
    public float speed;
    public float runSpeedMultiplier = 1.5f; // How much faster the player runs (e.g., 1.5x normal speed)

    public Vector3 CurrentDirection { get; private set; }
    public Vector3 LastNonZeroDirection => new Vector3(FacingDirection.x, FacingDirection.y, 0);
    public Vector2Int FacingDirection { get; private set; } = Vector2Int.down;
    public Vector3Int FacingCellOffset => new Vector3Int(FacingDirection.x, FacingDirection.y, 0);

    private float currentSpeed;

    private void Start()
    {
        currentSpeed = speed;
        UpdateAnimator(Vector3.zero);
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 inputDirection = new Vector2(horizontal, vertical);
        CurrentDirection = inputDirection.normalized;

        if (CurrentDirection.magnitude > 0)
        {
            FacingDirection = GetCardinalFacing(inputDirection);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = speed * runSpeedMultiplier;
        }
        else
        {
            currentSpeed = speed;
        }

        UpdateAnimator(CurrentDirection);
    }

    private void FixedUpdate()
    {
        transform.Translate(CurrentDirection * (currentSpeed * Time.deltaTime));
    }

    private Vector2Int GetCardinalFacing(Vector2 inputDirection)
    {
        if (Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y))
        {
            return inputDirection.x > 0 ? Vector2Int.right : Vector2Int.left;
        }

        if (Mathf.Abs(inputDirection.y) > 0)
        {
            return inputDirection.y > 0 ? Vector2Int.up : Vector2Int.down;
        }

        return FacingDirection;
    }

    private void UpdateAnimator(Vector3 direction)
    {
        if (animator != null)
        {
            if (direction.magnitude > 0)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            animator.SetFloat("horizontal", FacingDirection.x);
            animator.SetFloat("vertical", FacingDirection.y);
        }
    }
}
