using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainTowerController : MonoBehaviour
{
    public PathManager pathManager; // Reference to the PathManager
    public EnemySpawner enemySpawner; // Reference to the EnemySpawner
    public Terrain terrain; // Reference to the Terrain
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public GameObject healthBarPrefab; // Prefab for the health bar UI (Slider)

    public float shootingRange = 10f; // Range at which the tower can shoot
    public float damage = 10f; // Damage dealt by the tower's projectiles
    public float shootingInterval = 1f; // Time in seconds between each shot
    public float maxHealth = 100f; // Maximum health of the tower
    private float currentHealth; // Current health of the tower
    private float shootingTimer = 0f; // Timer to keep track of time since last shot
    public Transform projectileSpawnPoint;

    private GameObject healthBarInstance; // Instance of the health bar prefab
    private Slider healthBarSlider; // Reference to the slider component

    private void Start()
    {
        // Ensure the tower is placed on top of the terrain
        PositionTowerOnTerrain();

        currentHealth = maxHealth; // Initialize current health

        Debug.Log($"Tower starting position: {transform.position}");

        // Ensure that references are set
        if (pathManager == null) Debug.LogError("PathManager reference is missing.");
        if (enemySpawner == null) Debug.LogError("EnemySpawner reference is missing.");
        if (terrain == null) Debug.LogError("Terrain reference is missing.");
        if (projectilePrefab == null) Debug.LogError("ProjectilePrefab reference is missing.");
        if (projectileSpawnPoint == null) Debug.LogError("ProjectileSpawnPoint not found in the tower!");

        InstantiateHealthBar(); // Instantiate the health bar
        UpdateHealthBar(); // Initialize the health bar with current health
    }

    private void Update()
    {
        shootingTimer += Time.deltaTime;
        if (shootingTimer >= shootingInterval)
        {
            if (projectilePrefab != null)
            {
                ShootAtEnemies();
                shootingTimer = 0f; // Reset the timer after shooting
            }
        }
    }

    // Ensure that the tower is positioned correctly on top of the terrain
    private void PositionTowerOnTerrain()
    {
        // Sample the height of the terrain at the tower's current x and z positions
        float terrainHeight = terrain.SampleHeight(transform.position);

        // Adjust the tower's y position to match the terrain height
        transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z);
    }

    private void ShootAtEnemies()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, shootingRange);
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
        // Instantiate the projectile at the spawn point
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Calculate the direction to the target
        Vector3 direction = (target.position - projectile.transform.position).normalized;

        // Make the projectile look at the target, aligning the top of the bullet with the target
        projectile.transform.LookAt(target.position, Vector3.up);  // Adjust the 'up' vector to keep the top of the bullet upwards

        // Check if the projectile has the ProjectileController script
        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

        if (projectileController != null)
        {
            // Set the target and damage for the projectile
            projectileController.SetTarget(target, damage);
        }
        else
        {
            Debug.LogError("ProjectileController component missing on projectile prefab.");
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        // Clamp the health to ensure it doesn't go below 0
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update the health bar UI after taking damage
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void InstantiateHealthBar()
    {
        if (healthBarPrefab != null)
        {
            // Instantiate the health bar as a child of the tower
            healthBarInstance = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, transform);

            // Position the health bar above the tower (adjust y-axis as needed)
            healthBarInstance.transform.localPosition = new Vector3(0, 3.0f, 0); // Adjust this based on your tower's height

            // Get the Slider component from the instantiated health bar
            healthBarSlider = healthBarInstance.GetComponentInChildren<Slider>();

            if (healthBarSlider != null)
            {
                // Set the slider's max value to the tower's max health
                healthBarSlider.maxValue = maxHealth;
                healthBarSlider.value = currentHealth; // Initialize slider to current health
            }
            else
            {
                Debug.LogError("Slider component is missing from the health bar prefab.");
            }
        }
        else
        {
            Debug.LogError("Health bar prefab is not assigned.");
        }
    }

    private void UpdateHealthBar()
    {
        // Update the slider value based on the current health
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        // Load the end scene when the tower is destroyed
        SceneManager.LoadScene("EndScene");
        Debug.Log("MainTower down");
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
