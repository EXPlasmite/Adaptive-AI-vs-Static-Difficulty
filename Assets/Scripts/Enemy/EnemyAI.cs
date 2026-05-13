using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float baseSpeed = 2f;
    public float speed;
    private Transform player;

    void Start()
    {
        speed = baseSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }
}