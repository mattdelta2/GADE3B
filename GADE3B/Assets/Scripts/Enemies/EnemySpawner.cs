using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{/*
    public GameObject enemyPrefab;  // The enemy prefab
    public PathManager pathManager;  // Reference to PathManager
    public Terrain terrain;  // Reference to Terrain
    public float spawnInterval = 2f;  // Time between enemy spawns

    private float timeSinceLastSpawn;
    private Vector3[] spawnPoints;
    private bool spawningEnabled = false;

    void Start()
    {
        if (pathManager != null)
        {
            spawnPoints = pathManager.GenerateSpawnPoints();
        }
        else
        {
            Debug.LogError("PathManager is not assigned.");
        }
        timeSinceLastSpawn = 0f;
    }

    void Update()
    {
        if (spawningEnabled)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= spawnInterval)
            {
                SpawnEnemy();
                timeSinceLastSpawn = 0f;
            }
        }
    }

    public void StartSpawning()
    {
        spawningEnabled = true;
    }

    public void SpawnEnemy()
    {
        if (spawnPoints.Length > 0)
        {
            Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            if (terrain == null)
            {
                Debug.LogError("Terrain reference is missing.");
                return;
            }

            // Adjust spawn position to be on top of the terrain
            float terrainHeight = terrain.SampleHeight(spawnPoint);
            spawnPoint.y = terrainHeight;

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.terrain = terrain;  // Ensure terrain is assigned
                List<Vector3> path = pathManager.GeneratePath(spawnPoint); // Capture the returned path
                enemyController.SetPath(path); // Pass the path to the enemy controller
            }
            else
            {
                Debug.LogError("EnemyController component missing on enemy prefab.");
            }
        }
        else
        {
            Debug.LogError("No spawn points available.");
        }
    }








    
   
    public GameObject enemyPrefab;  // The enemy prefab
    public PathManager pathManager;  // Reference to PathManager
    public Terrain terrain;  // Reference to Terrain

    public float spawnInterval = 2f;  // Time between enemy spawns

    private float timeSinceLastSpawn;
    private Vector3[] spawnPoints;
    private bool spawningEnabled = false;

    void Start()
    {
        if (pathManager != null)
        {
            spawnPoints = pathManager.GenerateSpawnPoints();
        }
        timeSinceLastSpawn = 0f;
    }

    void Update()
    {
        if (spawningEnabled)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= spawnInterval)
            {
                SpawnEnemy();
                timeSinceLastSpawn = 0f;
            }
        }
    }

    public void StartSpawning()
    {
        spawningEnabled = true;
    }

    public void SpawnEnemy()
    {
        if (spawnPoints.Length > 0)
        {
            Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            // Adjust spawn position to be on top of the terrain
            float terrainHeight = terrain.SampleHeight(spawnPoint);
            spawnPoint.y = terrainHeight;
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            
            // Set enemy terrain reference and path
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetTerrain(terrain); // Set terrain reference
                enemyController.SetPath(pathManager.GeneratePath(spawnPoint));
            }
            else
            {
                Debug.LogError("EnemyController not found on the spawned enemy.");
            }
        }
    }







    public GameObject enemyPrefab;
    public Terrain terrain;
    public MainTowerController mainTowerController;
    private Vector3[] spawnPoints;
    private bool spawningEnabled = false;
    private float spawnInterval = 5f; // Time in seconds between spawns
    private float timer = 0f;

    private void Start()
    {
        spawnPoints = GenerateSpawnPoints();
        Debug.Log("Spawn points initialized.");
        StartSpawning();
    }

    private void Update()
    {
        if (spawningEnabled)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                timer = 0f;
                SpawnEnemy();
            }
        }
    }

    public void StartSpawning()
    {
        spawningEnabled = true;
        Debug.Log("Spawning enabled.");
    }

    private void SpawnEnemy()
    {
        if (spawningEnabled)
        {
            if (spawnPoints.Length > 0)
            {
                // Choose a random spawn point
                Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                float terrainHeight = terrain.SampleHeight(spawnPoint);
                spawnPoint.y = terrainHeight;  // Ensure spawn point is on the terrain

                GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
                Debug.Log($"Enemy spawned at: {spawnPoint}");

                // Set enemy terrain reference and path
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.SetTerrain(terrain);  // Set terrain reference

                    // Create a path to the tower
                    Vector3 towerPosition = mainTowerController.PlacedTower.transform.position;
                    List<Vector3> path = new List<Vector3> { towerPosition };
                    enemyController.SetPath(path);
                    Debug.Log($"Path set for enemy: {path}");
                }
                else
                {
                    Debug.LogError("EnemyController not found on the spawned enemy.");
                }
            }
            else
            {
                Debug.LogWarning("No spawn points available.");
            }
        }
        else
        {
            Debug.LogWarning("Spawning is not enabled.");
        }
    }

    private Vector3[] GenerateSpawnPoints()
    {
        // Example logic to generate 4 points around the edges
        List<Vector3> points = new List<Vector3>();

        Vector3 terrainCenter = new Vector3(terrain.terrainData.size.x / 2, 0, terrain.terrainData.size.z / 2);
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;

        points.Add(new Vector3(0, 0, terrainCenter.z)); // Left edge
        points.Add(new Vector3(terrainWidth, 0, terrainCenter.z)); // Right edge
        points.Add(new Vector3(terrainCenter.x, 0, 0)); // Bottom edge
        points.Add(new Vector3(terrainCenter.x, 0, terrainHeight)); // Top edge

        Debug.Log("Spawn points generated:");
        foreach (var point in points)
        {
            Debug.Log($"Spawn Point: {point}");
        }

        return points.ToArray();
    }*/



    public GameObject[] enemyPrefabs;  // Array of different enemy prefabs
    public Terrain terrain;  // Reference to the Terrain
    public MainTowerController mainTowerController;  // This will be assigned at runtime
    public float spawnInterval = 5f;  // Time in seconds between spawns
    public PathManager pathManager;  // Reference to the PathManager
    public TerrainGenerator terrainGenerator;  // Reference to TerrainGenerator for NavMesh readiness check

    private Vector3[] spawnPoints;
    private bool spawningEnabled = false;
    private float timer = 0f;
    private int spawnIndex = 0;

    private void Start()
    {
        // Start the process to wait for MainTowerController and NavMesh to be ready
        StartCoroutine(WaitForMainTowerControllerAndNavMesh());
    }

    private IEnumerator WaitForMainTowerControllerAndNavMesh()
    {
        // Wait until MainTowerController is assigned
        while (mainTowerController == null)
        {
            Debug.LogWarning("Waiting for MainTowerController to be assigned...");
            yield return null;  // Wait for the next frame
        }

        Debug.Log($"MainTowerController found at position: {mainTowerController.transform.position}");

        // Wait until the NavMesh is baked and ready
        yield return new WaitUntil(() => terrainGenerator != null && terrainGenerator.IsNavMeshReady());

        Debug.Log("NavMesh is ready, starting to generate spawn points...");

        if (pathManager != null)
        {
            spawnPoints = pathManager.GenerateSpawnPoints();

            // Ensure spawn points were generated
            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                Debug.LogError("No spawn points generated by PathManager.");
                yield break;  // Stop if no spawn points were generated
            }

            Debug.Log($"Generated {spawnPoints.Length} spawn points.");
            StartSpawning();  // Start the spawning process
        }
        else
        {
            Debug.LogError("PathManager reference is missing.");
        }
    }

    private void Update()
    {
        if (spawningEnabled)
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                timer = 0f;
                SpawnEnemy();
            }
        }
    }

    public void StartSpawning()
    {
        spawningEnabled = true;
        Debug.Log("Spawning enabled.");
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length > 0)
        {
            // Cycle through the spawn points using spawnIndex
            Vector3 spawnPoint = spawnPoints[spawnIndex];

            // Adjust Y position of the spawn point to terrain height
            if (terrain != null)
            {
                float originalY = spawnPoint.y;
                spawnPoint.y = terrain.SampleHeight(spawnPoint);
                Debug.Log($"Adjusted spawn point Y position from {originalY} to {spawnPoint.y} based on terrain height.");
            }

            // Check if the spawn point is on the NavMesh
            if (NavMesh.SamplePosition(spawnPoint, out NavMeshHit spawnHit, 10f, NavMesh.AllAreas))
            {
                spawnPoint = spawnHit.position;  // Use the NavMesh-adjusted position
                Debug.Log($"Spawning enemy at position: {spawnPoint}");

                // Choose a random enemy prefab to spawn
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
                Debug.Log($"Spawned enemy {enemy.name} at {spawnPoint}.");

                // Get the EnemyController and assign tower reference and NavMesh pathfinding
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.tower = mainTowerController.transform;

                    // Ensure the tower is on the NavMesh
                    if (NavMesh.SamplePosition(mainTowerController.transform.position, out NavMeshHit towerHit, 10f, NavMesh.AllAreas))
                    {
                        Vector3 validTowerPosition = towerHit.position;
                        Debug.Log($"Valid tower position on NavMesh: {validTowerPosition}");

                        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                        if (agent != null)
                        {
                            agent.SetDestination(validTowerPosition);
                            Debug.Log($"Assigned NavMeshAgent to move towards valid position: {validTowerPosition}");
                        }
                        else
                        {
                            Debug.LogError("NavMeshAgent not found on the enemy prefab.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Main tower is not on the NavMesh.");
                    }
                }
                else
                {
                    Debug.LogError("EnemyController not found on the spawned enemy.");
                }

                spawnIndex = (spawnIndex + 1) % spawnPoints.Length;  // Cycle to the next spawn point
            }
            else
            {
                Debug.LogWarning($"Spawn point {spawnPoint} is not on the NavMesh.");
            }
        }
        else
        {
            Debug.LogWarning("No spawn points available.");
        }
    }
}
