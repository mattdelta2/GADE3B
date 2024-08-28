using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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




    public GameObject[] enemyPrefabs;  // Array to hold different enemy prefabs
    public Terrain terrain;
    public MainTowerController mainTowerController;
    public PathManager pathManager;  // Reference to the PathManager
    private Vector3[] spawnPoints;
    private bool spawningEnabled = false;
    private float spawnInterval = 5f; // Time in seconds between spawns
    private float timer = 0f;
    private int spawnIndex = 0; // Keep track of the last spawn index

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
                // Ensure spawn point is valid
                Vector3 spawnPoint = spawnPoints[spawnIndex];
                float terrainHeight = terrain.SampleHeight(spawnPoint);
                spawnPoint.y = terrainHeight;  // Ensure spawn point is on the terrain

                Debug.Log($"Attempting to spawn enemy at: {spawnPoint}");

                // Choose a random enemy prefab to spawn
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
                Debug.Log($"Enemy instantiated at: {enemy.transform.position}");

                // Set enemy terrain reference and path
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.SetTerrain(terrain);  // Set terrain reference

                    // Get the path from the PathManager
                    List<Vector3> path = pathManager.GeneratePath(spawnPoint);
                    enemyController.SetPath(path);
                    Debug.Log($"Path set for enemy: {path}");
                }
                else
                {
                    Debug.LogError("EnemyController not found on the spawned enemy.");
                }

                // Move to the next spawn point
                spawnIndex = (spawnIndex + 1) % spawnPoints.Length;
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
    }
}