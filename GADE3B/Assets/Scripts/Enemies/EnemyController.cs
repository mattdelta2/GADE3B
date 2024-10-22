using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    // Existing fields
    public float health = 20f;               // Enemy health
    public float maxHealth = 20f;            // Max health of the enemy
    public float damageAmount = 10f;         // Damage amount the enemy can deal
    public float range = 10f;                // Range within which the enemy can attack
    public Transform tower;                  // Reference to the main tower
    public Transform currentTarget;          // Current target (either defender or tower)
    public GameObject projectilePrefab;      // Projectile prefab to shoot at the tower or defenders
    public Transform shootProjectile;        // Where the projectiles will spawn from
    public GameObject healthBarPrefab;       // Health bar prefab
    private GameObject healthBar;            // Instance of the health bar
    private Slider healthBarSlider;          // Slider component of the health bar

    protected NavMeshAgent agent;              // NavMeshAgent for pathfinding
    private float stuckTimer = 0f;           // Timer for tracking if the enemy is stuck
    private Vector3 lastPosition;            // Last recorded position of the enemy
    public float stuckThreshold = 10f;       // Time in seconds before considering an enemy stuck
    public EnemySpawner enemySpawner;        // Reference to the spawner for respawning
    private float shootingTimer = 0f;        // Timer for shooting intervals
    public float shootingInterval = 2f;      // Time between enemy attacks
    public float moveSpeed = 5f;             // Movement speed of the enemy

    protected virtual void Start()
    {
        // Initialize NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        lastPosition = transform.position;

        // Set initial target to the main tower
        currentTarget = tower;
        SetDestination(currentTarget);

        // Instantiate the health bar prefab above the enemy
        if (healthBarPrefab != null)
        {
            healthBar = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform);
            healthBarSlider = healthBar.GetComponentInChildren<Slider>();
            if (healthBarSlider != null)
            {
                healthBarSlider.maxValue = maxHealth;
                healthBarSlider.value = health;
            }
            else
            {
                Debug.LogError("Health bar prefab is missing a Slider component.");
            }
        }
        else
        {
            Debug.LogError("Health bar prefab is not assigned.");
        }

        // Ensure shootProjectile is assigned or attempt to find it in the children
        if (shootProjectile == null)
        {
            Transform potentialShootPoint = transform.Find("ShootPoint");
            if (potentialShootPoint != null)
            {
                shootProjectile = potentialShootPoint;
            }
            else
            {
                Debug.LogWarning("ShootProjectile is not assigned and no 'ShootPoint' child was found.");
            }
        }
    }

    protected virtual void Update()
    {
        // Check if the enemy is stuck and needs respawning
        CheckMovement();

        // Continuously search for defenders to attack
        FindClosestDefender();

        // Handle shooting at the current target
        shootingTimer += Time.deltaTime;
        if (currentTarget != null && IsTargetInRange(currentTarget) && shootingTimer >= shootingInterval)
        {
            ShootAtTarget(currentTarget);
            shootingTimer = 0f;  // Reset shooting timer after firing
        }

        // Handle health check
        if (health <= 0)
        {
            Die();
        }

        // Update health bar position to stay above the enemy
        if (healthBar != null)
        {
            healthBar.transform.position = transform.position + Vector3.up * 2;
        }
    }

    // New method for scaling enemy attributes
    public void ScaleAttributes(float healthScale, float damageScale, float speedScale)
    {
        health *= healthScale;
        maxHealth *= healthScale;
        damageAmount *= damageScale;
        moveSpeed *= speedScale;

        // Update health bar max value
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = health;
        }
    }

    // Method to check if the enemy is stuck
    private void CheckMovement()
    {
        if (agent.pathPending) return;

        if (agent.isPathStale)
        {
            RecalculatePath();
            return;
        }

        // Check if the enemy has moved
        if (Vector3.Distance(transform.position, lastPosition) < 0.1f && agent.velocity.magnitude < 0.1f)
        {
            stuckTimer += Time.deltaTime;

            // If stuck for too long, respawn the enemy
            if (stuckTimer >= stuckThreshold)
            {
                Debug.Log("Enemy is stuck for too long, respawning...");
                Respawn();
            }
        }
        else
        {
            stuckTimer = 0f;  // Reset stuck timer if the enemy has moved
        }

        lastPosition = transform.position;
    }

    // Method to find and prioritize defenders within range
    private void FindClosestDefender()
    {
        Collider[] hitDefenders = Physics.OverlapSphere(transform.position, range);
        float closestDistance = Mathf.Infinity;
        Transform closestDefender = null;

        // Find the closest defender within range
        foreach (Collider defenderCollider in hitDefenders)
        {
            if (defenderCollider.CompareTag("Defender"))
            {
                float distanceToDefender = Vector3.Distance(transform.position, defenderCollider.transform.position);
                if (distanceToDefender < closestDistance)
                {
                    closestDistance = distanceToDefender;
                    closestDefender = defenderCollider.transform;
                }
            }
        }

        // Set the target to the closest defender if found, otherwise fallback to the tower
        if (closestDefender != null)
        {
            currentTarget = closestDefender;
        }
        else
        {
            currentTarget = tower;
        }

        // Call SetDestination only if the NavMeshAgent is properly set up
        SetDestination(currentTarget);
    }

    // Method to set the NavMeshAgent's destination
    private void SetDestination(Transform target)
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                Debug.LogWarning("NavMeshAgent is not on the NavMesh. Retrying in 1 second.");
                StartCoroutine(RetrySetDestination(target));
            }
        }
        else
        {
            Debug.LogError("NavMeshAgent is either not initialized or not active.");
        }
    }

    // Coroutine to retry setting the destination after some delay
    private IEnumerator RetrySetDestination(Transform target)
    {
        yield return new WaitForSeconds(1f);

        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            Debug.LogWarning("NavMeshAgent still not on NavMesh after retry.");
        }
    }

    // Check if the current target is in range
    private bool IsTargetInRange(Transform target)
    {
        return Vector3.Distance(transform.position, target.position) <= range;
    }

    // Method to shoot projectiles at the current target (either defender or tower)
    private void ShootAtTarget(Transform target)
    {
        if (target != null && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootProjectile.position, Quaternion.identity);
            ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

            if (projectileController != null)
            {
                projectileController.SetTarget(target, damageAmount);
            }

            // Apply damage to the defender or tower
            if (target.CompareTag("Defender"))
            {
                DefenderController defender = target.GetComponent<DefenderController>();
                if (defender != null)
                {
                    defender.TakeDamage(damageAmount);
                }
            }
            else if (target.CompareTag("Tower"))
            {
                MainTowerController towerController = target.GetComponent<MainTowerController>();
                if (towerController != null)
                {
                    towerController.TakeDamage(damageAmount);
                }
            }
        }
    }

    // Method to handle when the enemy takes damage
    public void TakeDamage(float damage)
    {
        health -= damage;

        // Update the health bar slider
        if (healthBarSlider != null)
        {
            healthBarSlider.value = health;
        }

        // If health drops below zero, trigger the death
        if (health <= 0)
        {
            Die();
        }
    }

    // Method to check if the enemy is dead
    public bool IsDead()
    {
        return health <= 0;
    }

    // Method to handle the enemy's death
    private void Die()
    {
        // Notify the spawner or enemy manager that this enemy has died
        if (enemySpawner != null)
        {
            enemySpawner.OnEnemyDeath();
        }

        // Destroy the health bar when the enemy dies
        if (healthBar != null)
        {
            Destroy(healthBar);
        }

        // Destroy the enemy GameObject
        Destroy(gameObject);
    }

    // Method to handle respawning the enemy if it gets stuck
    private void Respawn()
    {
        // Notify the spawner that the enemy is being respawned
        if (enemySpawner != null)
        {
            enemySpawner.RespawnEnemy(gameObject);
        }

        // Destroy the current enemy instance
        Destroy(gameObject);
    }

    // Method to recalculate the enemy's path if necessary
    public void RecalculatePath()
    {
        if (agent != null && currentTarget != null)
        {
            agent.SetDestination(currentTarget.position);
        }
    }
}
