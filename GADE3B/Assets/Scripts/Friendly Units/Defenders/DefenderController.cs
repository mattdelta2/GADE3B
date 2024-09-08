using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenderController : MonoBehaviour
{
    public float range = 15f; // Range at which the defender can shoot
    public float damage = 20f; // Damage dealt by the defender's projectiles
    public float shootingInterval = 1.5f; // Time between shots
    public GameObject projectilePrefab; // Projectile prefab to be shot at enemies

    public float health = 50f; // Health of the defender
    public float maxHealth = 50f; // Max health for health bar
    public GameObject healthBarPrefab; // Health bar prefab
    private GameObject healthBar; // Instance of the health bar
    private Slider healthBarSlider; // Reference to the slider component

    private float shootingTimer = 0f;
    public Transform target; // Current enemy target
    public Transform shootProjectile;

    private void Start()
    {
        shootProjectile = transform.Find("shootProjectile");

        // Instantiate the health bar and set its max health
        healthBar = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform);
        healthBarSlider = healthBar.GetComponentInChildren<Slider>();
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = health;
    }

    private void Update()
    {
        shootingTimer += Time.deltaTime;
        if (shootingTimer >= shootingInterval)
        {
            ShootAtEnemy();
            shootingTimer = 0f; // Reset the timer after shooting
        }

        FindClosestEnemy(); // Continuously search for enemies

        // Update health bar position to stay above the defender
        if (healthBar != null)
        {
            healthBar.transform.position = transform.position + Vector3.up * 2;
        }
    }

    // Method to shoot at an enemy
    private void ShootAtEnemy()
    {

        if (target != null && projectilePrefab != null)
        {
            // Create the projectile and launch it at the enemy
            GameObject projectile = Instantiate(projectilePrefab, shootProjectile.position, Quaternion.identity);

            // Ensure the projectile faces the enemy
            projectile.transform.LookAt(target.position);

            // Optionally, adjust the rotation if the projectile is not facing correctly (depends on your projectile model)
            // For example, if your projectile needs to rotate along a certain axis:
            // projectile.transform.Rotate(90, 0, 0); // Adjust the values as per your model orientation

            ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
            if (projectileController != null)
            {
                // Set the target for the projectile
                projectileController.SetTarget(target);
            }
            else
            {
                Debug.LogError("ProjectileController component missing on projectile prefab.");
            }
        }
    }

    // Method to find the closest enemy in range
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
    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBarSlider.value = health; // Update health bar when taking damage
        if (health <= 0)
        {
            Die();
        }
    }

    // Method to handle defender's death
    private void Die()
    {
        Destroy(healthBar); // Destroy the health bar when the defender dies
        Destroy(gameObject); // Destroy the defender when health reaches zero
    }
}



