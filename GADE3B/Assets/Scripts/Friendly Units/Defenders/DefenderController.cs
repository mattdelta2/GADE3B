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

    public float shootingTimer = 0f;
    public Transform target;
    public Transform shootProjectile;

    private GameObject healthBar;
    private Slider healthBarSlider;

    [Header("Upgrade Settings")]
    public int upgradeLevel = 0; // Tracks current upgrade level
    public int maxUpgrades = 2; // Max number of upgrades
    public List<Sprite> upgradeSprites; // Sprites for each upgrade level
    public List<float> healthUpgrades; // Health increase for each level
    public List<float> damageUpgrades; // Damage increase for each level
    public List<float> rangeUpgrades;  // Range increase for each level
    public List<float> shootingIntervalUpgrades; // Shooting interval changes

    private SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        shootProjectile = transform.Find("shootProjectile");

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initialize health bar
        healthBar = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform);
        healthBarSlider = healthBar.GetComponentInChildren<Slider>();
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = health;

        UpdateAppearance(); // Set initial appearance
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
        if (upgradeLevel < maxUpgrades)
        {
            upgradeLevel++;

            // Apply upgrades
            if (upgradeLevel < healthUpgrades.Count)
            {
                maxHealth += healthUpgrades[upgradeLevel];
                health = maxHealth;
            }

            if (upgradeLevel < damageUpgrades.Count)
                damage += damageUpgrades[upgradeLevel];

            if (upgradeLevel < rangeUpgrades.Count)
                range += rangeUpgrades[upgradeLevel];

            if (upgradeLevel < shootingIntervalUpgrades.Count)
                shootingInterval -= shootingIntervalUpgrades[upgradeLevel];

            UpdateAppearance();
            Debug.Log($"Defender upgraded to level {upgradeLevel}. Health: {health}, Damage: {damage}, Range: {range}, Shooting Interval: {shootingInterval}");
        }
        else
        {
            Debug.Log("Maximum upgrade level reached.");
        }
    }

    private void UpdateAppearance()
    {
        if (spriteRenderer != null && upgradeLevel < upgradeSprites.Count)
        {
            spriteRenderer.sprite = upgradeSprites[upgradeLevel];
        }
    }

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
