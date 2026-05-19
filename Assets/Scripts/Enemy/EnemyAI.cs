using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float baseSpeed = 2f;
    public float speed;
    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        speed = baseSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
}