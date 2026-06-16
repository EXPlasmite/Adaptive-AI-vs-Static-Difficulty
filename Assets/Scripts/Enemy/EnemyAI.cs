using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles enemy movement and navigation towards the player.
// Uses A* pathfinding at longer range and switches to direct
// movement when within 2 units of the player.
// Movement speed is scaled by DifficultyManager's multiplier.
public class EnemyAI : MonoBehaviour
{
    public float baseSpeed = 2f;
    public float speed; // Set at spawn, scaled by difficulty multiplier
    private Transform player;
    private Rigidbody2D rb;
    private List<Node> path;
    private int targetIndex;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector3 lastPlayerPos;

    void Start()
    {
        speed = baseSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(UpdatePath());
    }

    // Recalculates path every 0.5 seconds, but only if the player
    // has moved more than 1 unit since the last calculation
    IEnumerator UpdatePath()
    {
        while (true)
        {
            if (player != null && Pathfinding.Instance != null)
            {
                if (Vector3.Distance(player.position, lastPlayerPos) > 1f ||
                    path == null)
                {
                    List<Node> newPath = Pathfinding.Instance.FindPath(
                        transform.position, player.position);
                    if (newPath != null && newPath.Count > 0)
                    {
                        path = newPath;
                        targetIndex = 0;
                        lastPlayerPos = player.position;
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Within 2 units: bypass pathfinding and move directly towards player
        if (Vector2.Distance(transform.position, player.position) < 2f)
        {
            Vector2 directDir = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + directDir * speed * Time.fixedDeltaTime);

            if (spriteRenderer != null)
            {
                if (directDir.x > 0) spriteRenderer.flipX = false;
                else if (directDir.x < 0) spriteRenderer.flipX = true;
            }

            if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                animator.Play("walk");
                animator.SetFloat("movementY", directDir.y > 0 ? 1f : -1f);
            }
            return;
        }

        // No valid path - play idle animation and wait for next path update
        if (path == null || path.Count == 0 || targetIndex >= path.Count)
        {
            path = null;
            if (animator != null) animator.Play("idle");
            return;
        }

        // Follow A* path node by node
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

        // Advance to next node when close enough to current target
        if (Vector2.Distance(transform.position, target) < 0.2f)
            targetIndex++;
    }
}