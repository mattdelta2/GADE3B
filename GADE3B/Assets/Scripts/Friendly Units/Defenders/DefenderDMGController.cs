using UnityEngine;
using UnityEngine.UI;

public class DefenderDMGController : MonoBehaviour
{
    [Header("Defender Attributes")]
    public float range = 12f; // Range at which the defender can shoot
    public float damage = 50f; // Higher damage for this defender
    public float shootingInterval = 0.5f; // Time between shots, faster shooting speed
    public GameObject projectilePrefab; // Projectile prefab to be shot at enemies

    [Header("Health Settings")]
    public float health = 50f; // Health of the defender
    public float maxHealth = 50f; // Max health for health bar
    public GameObject healthBarPrefab; // Health bar prefab
    private GameObject healthBar; // Instance of the health bar
    private Slider healthBarSlider; // Reference to the slider component

    [Header("Targeting")]
    public Transform target; // Current enemy target
    public Transform shootProjectile; // Where the projectiles are fired from

    private float shootingTimer = 0f; // Timer to control shooting frequency

    protected virtual void Start()
    {
        // Initialize shootProjectile reference
        shootProjectile = transform.Find("ShootPoint"); // Make sure there's an empty GameObject named "ShootPoint" on the defender prefab

        // Instantiate the health bar and set its max health
        if (healthBarPrefab != null)
        {
            healthBar = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform);
            healthBarSlider = healthBar.GetComponentInChildren<Slider>();
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = health;
        }
    }

    protected virtual void Update()
    {
        shootingTimer += Time.deltaTime;

        if (shootingTimer >= shootingInterval && target != null)
        {
            ShootAtEnemy();
            shootingTimer = 0f; // Reset the timer after shooting
        }

        // If the defender doesn't have a target, find the closest enemy
        if (target == null)
        {
            FindClosestEnemy();
        }

        // Update health bar position
        if (healthBar != null)
        {
            healthBar.transform.position = transform.position + Vector3.up * 2;
        }
    }

    private void ShootAtEnemy()
    {
        if (target != null && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootProjectile.position, Quaternion.identity);
            projectile.transform.LookAt(target.position);
            ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
            if (projectileController != null)
            {
                projectileController.SetTarget(target, damage);
            }
        }
    }

    private void FindClosestEnemy()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, range);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemyCollider.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemyCollider.transform;
                }
            }
        }

        target = closestEnemy;
    }

    // Method for the defender to take damage
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (healthBarSlider != null)
        {
            healthBarSlider.value = health; // Update health bar when taking damage
        }

        if (health <= 0)
        {
            Die();
        }
    }

    // Handle defender's death
    protected virtual void Die()
    {
        Destroy(healthBar); // Destroy the health bar when the defender dies
        Destroy(gameObject); // Destroy the defender when health reaches zero
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range); // Visualize range in Scene view
    }
}
