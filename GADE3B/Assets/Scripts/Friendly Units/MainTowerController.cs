using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float shootingRange = 10f; // Range at which the tower can shoot
    public float damage = 10f; // Damage dealt by the tower's projectiles

    public float shootingInterval = 1f; // Time in seconds between each shot
    private float shootingTimer = 0f;  // Timer to keep track of time since last shot


    private void Start()
    {
        // Ensure that references are set
        if (pathManager == null)
        {
            Debug.LogError("PathManager reference is missing.");
        }
        if (enemySpawner == null)
        {
            Debug.LogError("EnemySpawner reference is missing.");
        }
        if (terrain == null)
        {
            Debug.LogError("Terrain reference is missing.");
        }
        if (projectilePrefab == null)
        {
            Debug.LogError("ProjectilePrefab reference is missing.");
        }
    }

    private void Update()
    {
        shootingTimer += Time.deltaTime;
        if (shootingTimer >= shootingInterval)
        {
            if (projectilePrefab != null)
            {
                ShootAtEnemies();
                shootingTimer = 0f;  // Reset the timer after shooting
            }
        }
    }

    private void ShootAtEnemies()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, shootingRange);

        bool shooting = false;  // Flag to check if any enemies are hit

        foreach (Collider enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Enemy")) // Correctly using CompareTag to check the tag
            {
                shooting = true;  // Set flag to true if an enemy is hit
                                  // Example projectile launch
                LaunchProjectile(enemyCollider.transform);
            }
        }

        if (shooting)
        {
            Debug.Log("Tower is shooting at enemies.");
        }
    }

    private void LaunchProjectile(Transform target)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

        if (projectileController != null)
        {
            projectileController.SetTarget(target); // Set the target transform
        }
        else
        {
            Debug.LogError("ProjectileController component missing on projectile prefab.");
        }
    }

    // Call this method to apply damage to the tower
    public void TakeDamage(float damageAmount)
    {
        // Handle damage to the tower here
        Debug.Log("Tower took damage: " + damageAmount);
        // Add damage handling logic here, such as reducing health
    }
}





