using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainTowerController : MonoBehaviour
{/*
    public GameObject mainTowerPrefab;
    public PathManager pathManager;  // Reference to PathManager
    public EnemySpawner enemySpawner; // Reference to EnemySpawner
    public Terrain terrain; // Reference to the terrain
    public TowerPlacement towerPlacement;



    private bool canPlaceMainTower = true;
    private GameObject placedTower;  // Reference to the placed tower

    void Update()
    {
        if (canPlaceMainTower && Input.GetMouseButtonDown(0))  // Left click
        {
            Vector3 worldPosition = GetWorldPositionFromMouse();
            if (towerPlacement.CanPlaceTower(worldPosition))
            {
                // Place the main tower
                placedTower = Instantiate(mainTowerPrefab, worldPosition, Quaternion.identity);
                canPlaceMainTower = false;  // Prevent further placement

                // Notify PathManager to generate paths
                if (pathManager != null && placedTower != null)
                {
                    pathManager.SetTower(placedTower.transform); // Pass the actual tower's transform
                }
                else
                {
                    Debug.LogError("PathManager or placedTower is null.");
                }

                // Notify EnemySpawner to start spawning enemies
                if (enemySpawner != null)
                {
                    enemySpawner.StartSpawning(); // Start enemy spawning
                }
            }
            else
            {
                Debug.Log("Cannot place main tower here!");
            }
        }
    }

    Vector3 GetWorldPositionFromMouse()
    {
        // Convert mouse position to world position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }



   
    public GameObject mainTowerPrefab;
    public TowerPlacement towerPlacement;
    public PathManager pathManager;  // Reference to PathManager
    public EnemySpawner enemySpawner; // Reference to EnemySpawner
    public Terrain terrain; // Reference to the terrain

    private bool canPlaceMainTower = true;
    private GameObject placedTower;  // Reference to the placed tower

    void Update()
    {
        if (canPlaceMainTower && Input.GetMouseButtonDown(0))  // Left click
        {
            Vector3 worldPosition = GetWorldPositionFromMouse();
            if (towerPlacement.CanPlaceTower(worldPosition))
            {
                // Place the main tower
                Vector3 terrainPosition = new Vector3(worldPosition.x, terrain.SampleHeight(worldPosition), worldPosition.z);
                placedTower = Instantiate(mainTowerPrefab, terrainPosition, Quaternion.identity);
                canPlaceMainTower = false;  // Prevent further placement

                // Notify PathManager to generate paths
                if (pathManager != null && placedTower != null)
                {
                    pathManager.SetTower(placedTower.transform); // Pass the actual tower's transform
                }
                else
                {
                    Debug.LogError("PathManager or placedTower is null.");
                }

                // Notify EnemySpawner to start spawning enemies
                if (enemySpawner != null)
                {
                    enemySpawner.StartSpawning(); // Start enemy spawning
                }
            }
            else
            {
                Debug.Log("Cannot place main tower here!");
            }
        }
    }

    Vector3 GetWorldPositionFromMouse()
    {
        // Convert mouse position to world position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }





        //spawn tower by clicking the terrain
    
    public GameObject mainTowerPrefab;
    public TowerPlacement towerPlacement;
    public PathManager pathManager; // Reference to PathManager
    public EnemySpawner enemySpawner; // Reference to EnemySpawner
    public Terrain terrain; // Reference to the terrain

    private bool canPlaceMainTower = true;
    private GameObject placedTower; // Reference to the placed tower

    void Update()
    {
        if (canPlaceMainTower && Input.GetMouseButtonDown(0)) // Left click
        {
            // Get the center position of the terrain
            Vector3 terrainCenter = new Vector3(terrain.terrainData.size.x / 2, 0, terrain.terrainData.size.z / 2);
            float terrainHeight = terrain.SampleHeight(terrainCenter);
            Vector3 centerPosition = new Vector3(terrainCenter.x, terrainHeight, terrainCenter.z);

            // Place the main tower
            placedTower = Instantiate(mainTowerPrefab, centerPosition, Quaternion.identity);
            canPlaceMainTower = false; // Prevent further placement

            // Notify PathManager to generate paths
            if (pathManager != null && placedTower != null)
            {
                pathManager.SetTower(placedTower.transform); // Pass the actual tower's transform
            }
            else
            {
                Debug.LogError("PathManager or placedTower is null.");
            }

            // Notify EnemySpawner to start spawning enemies
            if (enemySpawner != null)
            {
                enemySpawner.StartSpawning(); // Start enemy spawning
            }
        }
    }*/

    //spawn tower when the game starts


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
        projectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
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

        // Check if the projectile has the ProjectileController script
        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

        if (projectileController != null)
        {
            // Make the projectile look at the target when it's instantiated
            projectile.transform.LookAt(target.position);

            // If needed, adjust the rotation to align the bullet's forward or top correctly
            // For example, if your bullet's top is facing up and needs to be rotated downwards:
            // projectile.transform.Rotate(90, 0, 0); // Adjust based on your bullet model's orientation

            // Set the target for the projectile
            projectileController.SetTarget(target);
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
        Time.timeScale = 0f;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}





