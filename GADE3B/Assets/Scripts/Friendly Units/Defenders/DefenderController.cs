using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenderController : MonoBehaviour
{
    [Header("Defender Type")]
    public string defenderType = "Normal";

    [Header("Combat Settings")]
    public float range;
    public float damage;
    public float shootingInterval = 1.5f;
    public GameObject projectilePrefab;

    [Header("Health Settings")]
    public float health = 50f;
    public float maxHealth = 50f;
    public GameObject healthBarPrefab;

    [Header("Upgrade Settings")]
    public int upgradeLevel = 0; // Tracks the current upgrade level
    public int maxUpgrades = 2; // Max number of upgrades
    public List<GameObject> upgradePrefabs; // List of prefabs for each upgrade level

    protected float shootingTimer = 0f; // Changed to protected
    protected Transform target;         // Changed to protected
    protected Transform shootProjectile;

    private GameObject healthBar;
    private Slider healthBarSlider;

    protected virtual void Start()
    {
        shootProjectile = transform.Find("shootProjectile");
        if (shootProjectile == null)
        {
            Debug.LogWarning("shootProjectile not found on Defender. Please ensure a child object named 'shootProjectile' exists.");
        }

        // Initialize health bar
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
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= range)
            {
                ShootAtEnemy();
                shootingTimer = 0f;
            }
            else
            {
                target = null;
            }
        }

        if (target == null)
        {
            FindClosestEnemy();
        }

        if (healthBar != null)
        {
            healthBar.transform.position = transform.position + Vector3.up * 2;
        }
    }

    public void UpgradeDefender()
    {
        if (upgradeLevel < maxUpgrades && upgradeLevel < upgradePrefabs.Count)
        {
            // Increment upgrade level
            upgradeLevel++;

            // Get the upgraded prefab for the current level
            GameObject upgradedPrefab = upgradePrefabs[upgradeLevel - 1]; // Upgrade level starts at 0, so use (level - 1)

            if (upgradedPrefab != null)
            {
                // Instantiate the upgraded defender at the same position and rotation
                GameObject upgradedDefender = Instantiate(upgradedPrefab, transform.position, transform.rotation, transform.parent);

                // Transfer current stats to the new defender
                DefenderController newDefender = upgradedDefender.GetComponent<DefenderController>();
                if (newDefender != null)
                {
                    newDefender.health = health;
                    newDefender.maxHealth = maxHealth;
                }

                // Destroy the current defender
                Destroy(gameObject);

                Debug.Log($"Defender upgraded to level {upgradeLevel} using prefab: {upgradedPrefab.name}");
            }
            else
            {
                Debug.LogWarning($"No prefab found for upgrade level {upgradeLevel}");
            }
        }
        else
        {
            Debug.Log("Maximum upgrade level reached or no prefab available.");
        }
    }

    protected void ShootAtEnemy()
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

    protected void FindClosestEnemy()
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

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        healthBarSlider.value = health;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void TargetEnemy(Transform enemy)
    {
        target = enemy;
        Debug.Log($"Defender is now targeting the enemy: {enemy.name}");
    }

    protected virtual void Die()
    {
        Destroy(healthBar);
        Destroy(gameObject);
    }
}
