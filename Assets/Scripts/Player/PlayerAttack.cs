using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDamage = 25f;
    public float spawnDistance = 1.5f;
    public float spawnHeightOffset = 0.5f;
    public KeyCode attackKey = KeyCode.Space;
    public float attackCooldown = 0.8f;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    private float lastAttackTime;
    private Animator animator;
    private AudioSource audioSource;
    private PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
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

        Vector2 direction = playerMovement.GetFacingDirection();
        arrowSpawnPoint.localPosition = new Vector3(
            direction.x * spawnDistance, 
            direction.y * spawnDistance + spawnHeightOffset, 
            0);

        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, 
                arrowSpawnPoint.position, Quaternion.identity);
            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                arrowScript.damage = attackDamage;
                arrowScript.Launch(direction);
            }
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}