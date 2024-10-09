using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenderController : MonoBehaviour
{
    [Header("Combat Settings")]
    public float range; // Range at which the defender can shoot, set in the Inspector
    public float damage; // Damage dealt by the defender's projectiles, set in the Inspector
    public float shootingInterval = 1.5f; // Time between shots, can be set in the Inspector
    public GameObject projectilePrefab; // Projectile prefab to be shot at enemies

    [Header("Health Settings")]
    public float health = 50f; // Health of the defender
    public float maxHealth = 50f; // Max health for health bar, set in the Inspector
    public GameObject healthBarPrefab; // Health bar prefab

    public float shootingTimer = 0f;
    public Transform target; // Current enemy target
    public Transform shootProjectile; // Where the projectile is shot from

    private GameObject healthBar; // Instance of the health bar
    private Slider healthBarSlider; // Reference to the slider component

    protected virtual void Start()
    {
        shootProjectile = transform.Find("shootProjectile");

        // Instantiate the health bar and set its max health
        healthBar = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform);
        healthBarSlider = healthBar.GetComponentInChildren<Slider>();
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = health;
    }

    protected virtual void Update()
    {
        shootingTimer += Time.deltaTime;

        if (shootingTimer >= shootingInterval && target != null)
        {
            ShootAtEnemies();
            shootingTimer = 0f; // Reset the timer after shooting
        }

        // If the defender doesn't have a forced target, find the closest enemy
        if (target == null)
        {
            FindClosestEnemy(); // Continuously search for enemies
        }

        // Update health bar position to stay above the defender
        if (healthBar != null)
        {
            healthBar.transform.position = transform.position + Vector3.up * 2;
        }
    }

    // Method to find the closest enemy in range
    private void FindClosestEnemy()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, range);
        Debug.Log("Defender range: " + range + " | Detected enemies: " + hitEnemies.Length); // Add this for debugging
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemyCollider.transform.position);
                Debug.Log("Enemy detected at distance: " + distanceToEnemy); // Add this for debugging
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemyCollider.transform;
                }
            }
        }

        target = closestEnemy;
    }

    // Method to shoot at an enemy
    private void ShootAtEnemies()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, range);
        bool shooting = false; // Flag to check if any enemies are hit

        foreach (Collider enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Enemy")) // Ensure the collider is an enemy
            {
                shooting = true; // Set flag to true if an enemy is hit

                // Launch projectile at the first enemy found
                LaunchProjectile(enemyCollider.transform);
                break; // Fire only one projectile per interval
            }
        }

        if (shooting)
        {
            Debug.Log("Tower is shooting at enemies.");
        }
    }
    private void LaunchProjectile(Transform target)
    {
        GameObject projectile = Instantiate(projectilePrefab, shootProjectile.position, Quaternion.identity);
        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

        if (projectileController != null)
        {
            projectile.transform.LookAt(target.position);
            projectileController.SetTarget(target, damage);
        }
    }


    // Method for the defender to take damage
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        healthBarSlider.value = health; // Update health bar when taking damage
        if (health <= 0)
        {
            Die();
        }
    }

    // Method to handle defender's death
    protected virtual void Die()
    {
        Destroy(healthBar); // Destroy the health bar when the defender dies
        Destroy(gameObject); // Destroy the defender when health reaches zero
    }

    public virtual void TargetEnemy(EnemyController enemy)
    {
        target = enemy.transform;
        Debug.Log("Defender is now targeting the taunting enemy: " + enemy.name);
    }
}
