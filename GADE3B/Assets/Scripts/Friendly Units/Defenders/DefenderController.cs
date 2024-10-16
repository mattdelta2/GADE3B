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

        // Shoot at the target if it's time to shoot and the target is in range
        if (shootingTimer >= shootingInterval && target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= range)
            {
                ShootAtEnemy();
                shootingTimer = 0f; // Reset the timer after shooting
            }
            else
            {
                // Target is out of range, find a new one
                target = null;
            }
        }

        // If the defender doesn't have a target, find the closest enemy
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

    // Method to shoot at the current target
    private void ShootAtEnemy()
    {
        if (projectilePrefab != null && target != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootProjectile.position, Quaternion.identity);
            ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

            if (projectileController != null)
            {
                projectile.transform.LookAt(target.position);
                projectileController.SetTarget(target, damage);
            }
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

    // Taunt behavior: make the defender target a specific enemy for a set duration
    public virtual void TargetEnemy(EnemyController enemy)
    {
        target = enemy.transform;
        Debug.Log("Defender is now targeting the taunting enemy: " + enemy.name);
    }
}
