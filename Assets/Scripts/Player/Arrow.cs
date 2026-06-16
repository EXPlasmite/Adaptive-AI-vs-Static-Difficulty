using UnityEngine;

// Handles arrow projectile behaviour including movement, collision
// and damage dealt to enemies. Player attack damage is fixed at 25f
// and is not affected by the difficulty multiplier - only enemy
// stats are scaled by DifficultyManager.
public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 25f;
    public float lifetime = 3f; // Arrow is destroyed after 3 seconds if it hits nothing
    private Vector2 direction;
    private AudioSource audioSource;
    private Rigidbody2D rb;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    // Sets the arrow's travel direction, called by the player attack script on fire
    public void Launch(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy arrow on contact with wall tiles
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
                enemy.TakeDamage(damage);

            if (audioSource != null)
            {
                // Play hit sound, hide sprite and disable colliders
                // before destroying to allow audio to complete
                audioSource.Play();
                GetComponent<SpriteRenderer>().enabled = false;
                foreach (Collider2D col in GetComponents<Collider2D>())
                    col.enabled = false;
                Destroy(gameObject, audioSource.clip.length);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}