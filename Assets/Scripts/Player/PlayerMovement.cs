using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 lastDirection = Vector2.down;

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

        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;
        else if (lastDirection == Vector2.right)
            spriteRenderer.flipX = false;
        else if (lastDirection == Vector2.left)
            spriteRenderer.flipX = true;

        if (animator == null) return;

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

    public Vector2 GetFacingDirection()
    {
        return lastDirection;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }
}