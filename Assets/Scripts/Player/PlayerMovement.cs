using UnityEngine;

// Handles player movement, sprite flipping and animation state.
// Also tracks the player's last facing direction, used by
// PlayerAttack to determine arrow spawn direction.
// Player speed is fixed and not affected by the difficulty multiplier.
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 lastDirection = Vector2.down; // Default facing direction on spawn

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = true;
    }

    void Update()
    {
        // Read raw input for movement (no smoothing)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Track last facing direction for arrow spawn and idle animation.
        // Prioritises the most recently pressed direction key.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            lastDirection = Vector2.up;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            lastDirection = Vector2.down;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow))
                lastDirection = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow))
                lastDirection = Vector2.left;
        }

        // Flip sprite horizontally based on movement or last facing direction
        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;
        else if (lastDirection == Vector2.right)
            spriteRenderer.flipX = false;
        else if (lastDirection == Vector2.left)
            spriteRenderer.flipX = true;

        if (animator == null) return;

        // Play walk or idle animation, avoiding interrupting attack animation
        if (movement.magnitude > 0)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
                animator.Play("walk");
            animator.SetFloat("movementY", movement.y > 0 ? 1f : -1f);
        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
                animator.Play("idle");
            animator.SetFloat("movementY", lastDirection == Vector2.up ? 1f : -1f);
        }
    }

    // Returns the last direction the player was moving in.
    // Called by PlayerAttack to determine arrow launch direction.
    public Vector2 GetFacingDirection()
    {
        return lastDirection;
    }

    void FixedUpdate()
    {
        // Apply normalised movement to prevent faster diagonal movement
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }
}