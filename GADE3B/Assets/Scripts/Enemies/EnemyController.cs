using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float health = 20f;               
    public float maxHealth = 20f;            
    public float damageAmount = 10f;         
    public float range = 10f;                
    public Transform tower;                  
    public Transform currentTarget;          
    public GameObject projectilePrefab;      
    public Transform shootProjectile;        
    public GameObject healthBarPrefab;       
    private GameObject healthBar;            
    private Slider healthBarSlider;          

    protected NavMeshAgent agent;             
    private float stuckTimer = 0f;           
    private Vector3 lastPosition;            
    public float stuckThreshold = 10f;       
    public EnemySpawner enemySpawner;        
    private float shootingTimer = 0f;        
    public float shootingInterval = 2f;      
    public float moveSpeed = 5f;             

    protected virtual void Start()
    {
        // Initialize NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        lastPosition = transform.position;

        // Start coroutine to ensure NavMeshAgent is ready
        StartCoroutine(InitializeAgent());

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

    private IEnumerator InitializeAgent()
    {
        // Wait until the NavMeshAgent is on the NavMesh
        yield return new WaitUntil(() => agent.isOnNavMesh);

        // Set initial target to the main tower once agent is on the NavMesh
        currentTarget = tower;
        SetDestination(currentTarget);
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

    // Method for scaling enemy attributes
    public virtual void ScaleAttributes(float healthScale, float damageScale, float speedScale)
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

    private void CheckMovement()
    {
        if (agent.pathPending) return;

        if (agent.isPathStale)
        {
            RecalculatePath();
            return;
        }

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

    private void FindClosestDefender()
    {
        Collider[] hitDefenders = Physics.OverlapSphere(transform.position, range);
        float closestDistance = Mathf.Infinity;
        Transform closestDefender = null;

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

        if (closestDefender != null)
        {
            currentTarget = closestDefender;
        }
        else
        {
            currentTarget = tower;
        }

        SetDestination(currentTarget);
    }

    private void SetDestination(Transform target)
    {
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.SetDestination(target.position);
        }
    }

    private bool IsTargetInRange(Transform target)
    {
        return Vector3.Distance(transform.position, target.position) <= range;
    }

    private void ShootAtTarget(Transform target)
    {
        if (target != null && projectilePrefab != null && shootProjectile != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootProjectile.position, Quaternion.identity);
            EnemyProjectileController projectileController = projectile.GetComponent<EnemyProjectileController>();

            if (projectileController != null)
            {
                projectileController.SetTarget(target);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (healthBarSlider != null)
        {
            healthBarSlider.value = health;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    private void Die()
    {
        if (enemySpawner != null)
        {
            enemySpawner.OnEnemyDeath();
        }

        if (healthBar != null)
        {
            Destroy(healthBar);
        }

        Destroy(gameObject);
    }

    private void Respawn()
    {
        if (enemySpawner != null)
        {
            enemySpawner.RespawnEnemy(gameObject);
        }

        Destroy(gameObject);
    }

    public void RecalculatePath()
    {
        if (agent != null && currentTarget != null)
        {
            agent.SetDestination(currentTarget.position);
        }
    }
}
