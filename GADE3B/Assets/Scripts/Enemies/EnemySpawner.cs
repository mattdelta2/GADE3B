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
    public DefenderManager defenderManager;  // Reference to manage defenders

    [Header("Spawn Settings")]
    public int numberOfSpawnPoints = 8;  // Number of spawn points around the tower
    public float minimumSpawnDistanceFromTower = 20f;  // Minimum distance from the tower to spawn enemies
    public float defenderCheckRadius = 10f;  // Distance to check for nearby defenders to avoid spawning near them

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
    public float playerHealth;  // Simulated player health (to be tracked)

    // Defender-related spawn chances
    private Dictionary<string, float[]> defenderTypeSpawnChances = new Dictionary<string, float[]>();

    // Track the types of defenders placed
    private Dictionary<string, int> defenderTypesCount = new Dictionary<string, int>();

    private void Start()
    {
        StartCoroutine(WaitForMainTowerControllerAndNavMesh());

        // Initialize the spawn chances based on defender types
        InitializeDefenderTypeSpawnChances();
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

            // Check for nearby defenders to avoid spawning in heavily fortified areas
            if (IsNearDefender(spawnPosition))
                continue;

            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                spawnPoints.Add(hit.position);
            }
        }

        Debug.Log("Spawn points generated: " + spawnPoints.Count);
    }

    private bool IsNearDefender(Vector3 spawnPosition)
    {
        Collider[] defendersNearby = Physics.OverlapSphere(spawnPosition, defenderCheckRadius);
        foreach (Collider col in defendersNearby)
        {
            if (col.CompareTag("Defender"))
            {
                return true;  // Spawn point is too close to a defender
            }
        }
        return false;
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

        // Adjust the y position based on the terrain height at the x and z position
        spawnPoint.y = terrain.SampleHeight(spawnPoint);

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
                    // Warp the agent to ensure it's placed correctly on the terrain
                    agent.Warp(spawnPoint);
                    agent.SetDestination(mainTowerController.transform.position);

                    // Stuck detection and fallback
                    StartCoroutine(CheckIfStuck(agent, enemyController));
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
        string mostCommonDefenderType = defenderManager.GetMostCommonDefenderType();
        Debug.Log("Most common defender type: " + mostCommonDefenderType);

        if (!defenderTypeSpawnChances.ContainsKey(mostCommonDefenderType))
        {
            Debug.LogError("No spawn chances found for defender type: " + mostCommonDefenderType);
            return null; // Safely return to avoid further null reference errors
        }

        float[] spawnChances = defenderTypeSpawnChances[mostCommonDefenderType];
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= spawnChances[0]) // Tank enemy spawn chance
        {
            return enemyPrefabs[0]; // Assume the first prefab is Tank
        }
        else if (randomValue <= spawnChances[0] + spawnChances[1]) // Speed enemy spawn chance
        {
            return enemyPrefabs[1]; // Assume the second prefab is Speed
        }
        else // Normal enemy spawn chance
        {
            return enemyPrefabs[2]; // Assume the third prefab is Normal
        }
    }


    // Initialize spawn chances for different defender types
    private void InitializeDefenderTypeSpawnChances()
    {
        defenderTypeSpawnChances.Add("Normal", new float[] { 0.15f, 0.1f, 0.75f });
        defenderTypeSpawnChances.Add("AOE", new float[] { 0.10f, 0.65f, 0.25f });
        defenderTypeSpawnChances.Add("DMG", new float[] { 0.65f, 0.3f, 0.05f });

        Debug.Log("Initialized defender spawn chances. Count: " + defenderTypeSpawnChances.Count);
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

    private void AdaptDifficulty()
    {
        // Adjust player skill level based on performance
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

    private IEnumerator CheckIfStuck(NavMeshAgent agent, EnemyController enemyController)
    {
        Vector3 lastPosition = agent.transform.position;
        float stuckTimer = 0f;
        float stuckThreshold = 2f;  // Time in seconds to consider an enemy stuck

        while (agent != null && !agent.isStopped)
        {
            yield return new WaitForSeconds(0.5f);  // Check every half second

            if (Vector3.Distance(lastPosition, agent.transform.position) < 0.1f)
            {
                stuckTimer += 0.5f;  // Add half a second to the stuck timer
                if (stuckTimer >= stuckThreshold)
                {
                    Debug.LogWarning($"Enemy {enemyController.gameObject.name} is stuck, respawning...");
                    RespawnEnemy(enemyController.gameObject);
                    yield break;  // Exit the coroutine if the enemy is stuck
                }
            }
            else
            {
                stuckTimer = 0f;  // Reset the timer if the enemy is moving
            }

            lastPosition = agent.transform.position;
        }
    }

    // Method to add a defender type when it's placed
    public void AddDefenderType(string defenderType)
    {
        if (defenderTypesCount.ContainsKey(defenderType))
        {
            defenderTypesCount[defenderType]++;
        }
        else
        {
            defenderTypesCount[defenderType] = 1;
        }

        Debug.Log($"Defender of type {defenderType} added. Current count: {defenderTypesCount[defenderType]}");
    }

    // Method to log defender type counts (optional, for debugging)
    public void LogDefenderTypeCounts()
    {
        foreach (var defenderType in defenderTypesCount)
        {
            Debug.Log($"{defenderType.Key}: {defenderType.Value} placed.");
        }
    }

    public void SetPlayerSkillLevel(float newSkillLevel)
    {
        playerSkillLevel = Mathf.Max(1f, newSkillLevel);
        Debug.Log($"Player skill level set to: {playerSkillLevel}");
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
}
