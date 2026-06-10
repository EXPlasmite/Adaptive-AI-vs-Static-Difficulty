using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDamage = 25f;
    public float attackRange = 1.5f;
    public KeyCode attackKey = KeyCode.Space;
    public float attackCooldown = 0.8f;
    private float lastAttackTime;
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;

        if (animator != null)
            animator.Play("attack");

        if (audioSource != null)
            audioSource.Play();

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position, attackRange);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyController enemy = hit.GetComponent<EnemyController>();
                if (enemy != null)
                    enemy.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}