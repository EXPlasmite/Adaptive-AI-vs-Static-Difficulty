using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 facingDirection = Vector2.down;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = true;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.magnitude > 0)
            facingDirection = movement.normalized;

        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;

        if (animator == null) return;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            return;

        if (movement.magnitude > 0)
        {
            animator.Play("walk");
            animator.SetFloat("movementY", movement.y > 0 ? 1f : -1f);
        }
        else
        {
            animator.Play("idle");
            animator.SetFloat("movementY", -1f);
        }
    }

    public Vector2 GetFacingDirection()
    {
        // Prioritise most recent movement direction
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            return spriteRenderer.flipX ? Vector2.left : Vector2.right;
        else if (movement.y > 0)
            return Vector2.up;
        else if (movement.y < 0)
            return Vector2.down;
        else
            return spriteRenderer.flipX ? Vector2.left : Vector2.right;
    }

    public bool GetSpriteFlip()
    {
        return spriteRenderer.flipX;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }
}