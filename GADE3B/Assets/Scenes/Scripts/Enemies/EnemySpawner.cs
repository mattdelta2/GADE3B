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
    }*/





    
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
            float terrainHeight = terrain.SampleHeight(spawnPoint);
            spawnPoint.y = terrainHeight;  // Ensure spawn point is on the terrain

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();

            if (enemyController != null)
            {
                enemyController.SetTerrain(terrain);  // Set terrain reference
                enemyController.SetPath(pathManager.GeneratePath(spawnPoint));  // Set path to move towards the tower
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
}
