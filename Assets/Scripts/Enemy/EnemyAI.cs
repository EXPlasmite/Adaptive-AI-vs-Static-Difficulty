using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    public float baseSpeed = 2f;
    public float speed;
    private Transform player;
    private Rigidbody2D rb;
    private List<Node> path;
    private int targetIndex;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        speed = baseSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {
        while (true)
        {
            if (player != null && Pathfinding.Instance != null)
            {
                List<Node> newPath = Pathfinding.Instance.FindPath(
                    transform.position, player.position);
                if (newPath != null && newPath.Count > 0)
                {
                    path = newPath;
                    targetIndex = 0;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        if (path == null || path.Count == 0 || targetIndex >= path.Count)
        {
            path = null;
            if (animator != null) animator.Play("idle");
            return;
        }

        Vector2 target = path[targetIndex].worldPosition;
        Vector2 direction2 = (target - (Vector2)transform.position).normalized;
        rb.MovePosition(rb.position + direction2 * speed * Time.fixedDeltaTime);

        if (spriteRenderer != null)
        {
            if (direction2.x > 0)
                spriteRenderer.flipX = false;
            else if (direction2.x < 0)
                spriteRenderer.flipX = true;
        }

        if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            animator.Play("walk");
            animator.SetFloat("movementY", direction2.y > 0 ? 1f : -1f);
        }

        if (Vector2.Distance(transform.position, target) < 0.2f)
            targetIndex++;
    }
}