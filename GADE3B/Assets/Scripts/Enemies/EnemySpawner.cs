using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public MainTowerController mainTowerController;  // Reference to the tower
    public float spawnInterval = 2f;  // Time between enemy spawns within a wave
    public float timeBetweenWaves = 5f;  // Time between waves
    public int enemiesPerWave = 5;  // Number of enemies per wave
    public PathManager pathManager;  // Reference to the PathManager for spawn points
    public TerrainGenerator terrainGenerator;  // For NavMesh readiness check
    public TextMeshProUGUI waveText;  // Reference to the UI Text for displaying the wave number

    private List<Vector3> spawnPoints = new List<Vector3>();
    private bool spawningEnabled = false;
    private int currentWave = 1;
    private int enemiesSpawned = 0;
    private int enemiesInCurrentWave = 0;  // Track the number of enemies in the current wave

    public EnemyManager enemyManager;  // Reference to EnemyManager

    private void Start()
    {
        StartCoroutine(WaitForMainTowerControllerAndNavMesh());
        ApplyDifficultySettings();
    }

    private IEnumerator WaitForMainTowerControllerAndNavMesh()
    {
        while (mainTowerController == null)
        {
            Debug.LogWarning("Waiting for MainTowerController to be assigned...");
            yield return null;  // Wait for the next frame
        }

        yield return new WaitUntil(() => terrainGenerator != null && terrainGenerator.IsNavMeshReady());

        Debug.Log("NavMesh is ready, generating spawn points...");
        GenerateSpawnPoints();

        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points generated.");
            yield break;  // Stop if no spawn points
        }

        StartSpawning();  // Start the spawning process
    }

    public void GenerateSpawnPoints()
    {
        spawnPoints.Clear();
        Vector3 terrainSize = terrain.terrainData.size;

        // Define a buffer zone around the edges where enemies can spawn
        float buffer = 10f; // Adjust this value to control how close to the edge enemies can spawn

        // Number of spawn points you want to generate
        int numberOfSpawnPoints = 8;

        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            // Randomly choose whether to spawn on the left/right or top/bottom edges
            bool isHorizontalEdge = Random.value > 0.5f;
            Vector3 spawnPosition;

            if (isHorizontalEdge)
            {
                // Spawn on the left or right edge
                float xPosition = Random.value > 0.5f ? buffer : terrainSize.x - buffer;
                float zPosition = Random.Range(buffer, terrainSize.z - buffer);
                spawnPosition = new Vector3(xPosition, 0, zPosition);
            }
            else
            {
                // Spawn on the top or bottom edge
                float xPosition = Random.Range(buffer, terrainSize.x - buffer);
                float zPosition = Random.value > 0.5f ? buffer : terrainSize.z - buffer;
                spawnPosition = new Vector3(xPosition, 0, zPosition);
            }

            // Adjust the spawn position to be on the terrain's surface
            float terrainHeight = terrain.SampleHeight(spawnPosition);
            spawnPosition.y = terrainHeight;

            // Ensure the spawn point is on the NavMesh
            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                spawnPoints.Add(hit.position);
            }
        }

        Debug.Log("Spawn points generated near the edges: " + spawnPoints.Count);
    }


    public void StartSpawning()
    {
        spawningEnabled = true;
        UpdateWaveText();  // Update the wave text before starting the first wave
        StartCoroutine(SpawnWave());  // Start the first wave
    }

    private IEnumerator SpawnWave()
    {
        Debug.Log($"Starting wave {currentWave}");

        enemiesSpawned = 0;  // Reset for the new wave
        enemiesInCurrentWave = enemiesPerWave;  // Set the total number of enemies in this wave

        UpdateWaveText();  // Update the wave number in the UI

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();  // Spawn an enemy
            enemiesSpawned++;

            yield return new WaitForSeconds(spawnInterval);  // Delay between enemies
        }

        currentWave++;  // Move to the next wave
        yield return new WaitForSeconds(timeBetweenWaves);  // Wait before the next wave

        StartCoroutine(SpawnWave()); // Start the next wave
    }

    public void SpawnEnemy()
    {
        if (spawnPoints.Count == 0) return;

        // Choose a spawn point
        Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Ensure spawn point is on NavMesh
        if (NavMesh.SamplePosition(spawnPoint, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            spawnPoint = hit.position;  // Adjust to valid NavMesh point

            // Determine which enemy types to spawn based on the current wave number
            int maxEnemyIndex = currentWave <= 1 ? 1 : currentWave == 2 ? 2 : enemyPrefabs.Length;
            int randomEnemyIndex = Random.Range(0, Mathf.Min(maxEnemyIndex, enemyPrefabs.Length));
            GameObject enemyPrefab = enemyPrefabs[randomEnemyIndex];

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

            // Track active enemies in EnemyManager
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.tower = mainTowerController.transform;
                enemyController.enemySpawner = this;  // Pass reference to spawner for respawning

                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.SetDestination(mainTowerController.transform.position);
                }

                // Add enemy to EnemyManager
                enemyManager.AddEnemy(enemyController);
            }
        }
        else
        {
            Debug.LogError("Spawn point is not valid on the NavMesh.");
        }
    }

    private void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
        }
        else
        {
            Debug.LogError("WaveText UI reference is not assigned.");
        }
    }

    public void OnEnemyDeath()
    {
        enemiesInCurrentWave--;

        // Check if all enemies in the current wave are dead
        if (enemiesInCurrentWave <= 0)
        {
            StartCoroutine(SpawnWave());  // Start the next wave
        }
    }

    public void RespawnEnemy(GameObject enemy)
    {
        // Remove the enemy from the EnemyManager
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyManager.RemoveEnemy(enemyController);
        }

        // Destroy the old enemy
        Destroy(enemy);

        // Spawn a new enemy to replace the one that was removed
        SpawnEnemy();
    }

    public bool isTestingMode = true; // Flag to toggle testing mode

    private void ApplyDifficultySettings()
    {
        string selectedDifficulty = PlayerPrefs.GetString("SelectedDifficulty", "Normal"); // Retrieve the selected difficulty

        if (isTestingMode)
        {
            // Use the same settings for all difficulties during testing
            spawnInterval = 2f;
            enemiesPerWave = 5;
            timeBetweenWaves = 5f;
        }
        else
        {
            // Use different settings based on the selected difficulty
            if (selectedDifficulty == "Normal")
            {
                spawnInterval = 2f;
                enemiesPerWave = 5;
                timeBetweenWaves = 5f;
            }
            else if (selectedDifficulty == "Hard")
            {
                spawnInterval = 1f;
                enemiesPerWave = 10;
                timeBetweenWaves = 3f;
            }
        }
        Debug.Log("Difficulty settings applied: " + (isTestingMode ? "Testing mode" : selectedDifficulty));
    }
}
