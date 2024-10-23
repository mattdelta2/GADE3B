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

    [Header("Spawn Settings")]
    public int numberOfSpawnPoints = 8;  // Number of spawn points around the tower
    public float minimumSpawnDistanceFromTower = 20f;  // Minimum distance from the tower to spawn enemies

    private List<Vector3> spawnPoints = new List<Vector3>();
    private bool spawningEnabled = false;
    private int currentWave = 1;
    private int enemiesSpawned = 0;
    private int enemiesInCurrentWave = 0;  // Track the number of enemies in the current wave
    public TextMeshProUGUI waveText;

    public EnemyManager enemyManager;  // Reference to EnemyManager

    // Adaptive Spawning Metrics
    private int enemiesDefeatedInWave = 0;
    private float playerSkillLevel = 1f;  // Skill level indicator (1 means normal, >1 means good, <1 means struggling)

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

        Vector3 towerPosition = mainTowerController.transform.position;

        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * terrainSize.x * 0.5f;
            randomDirection.y = 0;
            Vector3 spawnPosition = towerPosition + randomDirection;

            float terrainHeight = terrain.SampleHeight(spawnPosition);
            spawnPosition.y = terrainHeight;

            // Ensure enemies do not spawn too close to the main tower
            if (Vector3.Distance(spawnPosition, towerPosition) < minimumSpawnDistanceFromTower)
                continue;

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
        StartCoroutine(SpawnWave());  // Start the first wave
    }

    private IEnumerator SpawnWave()
    {
        Debug.Log($"Starting wave {currentWave}");

        enemiesSpawned = 0;
        enemiesDefeatedInWave = 0;  // Reset defeated enemies count for new wave
        waveText.text = "Wave: " + currentWave.ToString();

        // Calculate the number of enemies for the current wave
        enemiesInCurrentWave = Mathf.RoundToInt(baseEnemiesPerWave * playerSkillLevel);

        for (int i = 0; i < enemiesInCurrentWave; i++)
        {
            SpawnEnemy();  // Spawn an enemy
            enemiesSpawned++;

            float adjustedSpawnInterval = spawnInterval / playerSkillLevel;
            yield return new WaitForSeconds(adjustedSpawnInterval);  // Adjust interval by skill level
        }

        currentWave++;
        yield return new WaitForSeconds(timeBetweenWaves);

        AdaptDifficulty();  // Adjust difficulty based on performance
        StartCoroutine(SpawnWave());  // Start the next wave
    }

    public void SpawnEnemy()
    {
        if (spawnPoints.Count == 0) return;

        Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        if (NavMesh.SamplePosition(spawnPoint, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            spawnPoint = hit.position;

            GameObject enemyPrefab = SelectEnemyTypeForWave();
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.tower = mainTowerController.transform;
                enemyController.enemySpawner = this;

                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.SetDestination(mainTowerController.transform.position);
                }

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
        if (currentWave < 4)
        {
            return enemyPrefabs[0];
        }
        else if (currentWave < 7)
        {
            return enemyPrefabs[Random.Range(0, 2)];
        }
        else
        {
            return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        }
    }

    public void OnEnemyDeath()
    {
        enemiesInCurrentWave--;
        enemiesDefeatedInWave++;

        if (enemiesInCurrentWave <= 0)
        {
            Debug.Log($"Wave {currentWave - 1} complete. Enemies defeated: {enemiesDefeatedInWave}");
        }
    }

    public void RespawnEnemy(GameObject enemy)
    {
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyManager.RemoveEnemy(enemyController);
        }

        Destroy(enemy);
        SpawnEnemy();
    }

    public void SetEnemyDifficultyScale(float difficultyScale)
    {
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

    public void SetPlayerSkillLevel(float newSkillLevel)
    {
        playerSkillLevel = Mathf.Max(1f, newSkillLevel);
        Debug.Log($"Player skill level set to: {playerSkillLevel}");
    }

    private void AdaptDifficulty()
    {
        if (enemiesDefeatedInWave > enemiesInCurrentWave * 0.8f)
        {
            playerSkillLevel += 0.1f;  // Increase skill level if player is doing well
        }
        else if (enemiesDefeatedInWave < enemiesInCurrentWave * 0.5f)
        {
            playerSkillLevel = Mathf.Max(1f, playerSkillLevel - 0.1f);  // Decrease skill level if struggling
        }

        Debug.Log($"Player skill level adjusted to: {playerSkillLevel}");
    }
}
