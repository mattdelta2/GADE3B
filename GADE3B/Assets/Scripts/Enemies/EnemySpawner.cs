using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Array of different enemy prefabs
    public Terrain terrain;  // Reference to the Terrain
    public MainTowerController mainTowerController;  // Reference to the tower
    public float spawnInterval = 2f;  // Time between enemy spawns within a wave
    public float timeBetweenWaves = 5f;  // Time between waves
    public int baseEnemiesPerWave = 5;  // Base number of enemies per wave
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

        // Generate random points around the main tower
        Vector3 towerPosition = mainTowerController.transform.position;
        int numberOfSpawnPoints = 8;  // Customize the number of spawn points

        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            // Generate random direction and distance
            Vector3 randomDirection = Random.insideUnitSphere * terrainSize.x * 0.5f;
            randomDirection.y = 0;  // Keep on the ground level
            Vector3 spawnPosition = towerPosition + randomDirection;

            // Adjust spawn position to be on the terrain
            float terrainHeight = terrain.SampleHeight(spawnPosition);
            spawnPosition.y = terrainHeight;

            // Ensure the spawn point is on the NavMesh
            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                spawnPoints.Add(hit.position);
            }
        }

        Debug.Log("Spawn points generated: " + spawnPoints.Count);
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

        // Calculate the number of enemies for the current wave
        enemiesInCurrentWave = baseEnemiesPerWave + (currentWave - 1);  // Increase enemies as waves progress

        UpdateWaveText();  // Update the wave number in the UI

        for (int i = 0; i < enemiesInCurrentWave; i++)
        {
            SpawnEnemy();  // Spawn an enemy
            enemiesSpawned++;

            yield return new WaitForSeconds(spawnInterval);  // Delay between enemies
        }

        currentWave++;  // Move to the next wave
        yield return new WaitForSeconds(timeBetweenWaves);  // Wait before the next wave

        SetEnemyDifficultyScale(currentWave * 0.1f);  // Increase difficulty for the next wave
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

            // Choose an enemy type based on the current wave
            GameObject enemyPrefab = SelectEnemyTypeForWave();

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

    private GameObject SelectEnemyTypeForWave()
    {
        // Determine which enemy type(s) to spawn based on the current wave number
        if (currentWave < 4)
        {
            return enemyPrefabs[0];  // Only spawn Enemy Type 1
        }
        else if (currentWave < 7)
        {
            // Spawn either Enemy Type 1 or Type 2
            return enemyPrefabs[Random.Range(0, 2)];
        }
        else
        {
            // Spawn any of the three enemy types
            return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
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

    public void SetEnemyDifficultyScale(float difficultyScale)
    {
        // Apply scaling to all enemy parameters
        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            EnemyController enemyController = enemyPrefab.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.ScaleAttributes(difficultyScale, difficultyScale, difficultyScale);
            }
            else
            {
                Debug.LogError("EnemyController not found on enemy prefab.");
            }
        }
    }

}
