using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

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

        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;

        if (animator == null) return;

        // Don't override attack animation
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

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }
}