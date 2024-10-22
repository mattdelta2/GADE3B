using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject towerPrefab;
    public PathManager pathManager;
    public EnemySpawner enemySpawner;
    public Terrain terrain;
    public GameObject projectilePrefab;
    public FollowCamera cameraController;
    public TerrainGenerator terrainGenerator;
    public DefenderPlacementManager defenderPlacementManager;
    private NavMeshSurface navMeshSurface;

    private GameObject tower; // Store reference to the instantiated tower

    // Difficulty settings
    public float normalSpawnInterval = 2f;
    public float hardSpawnInterval = 1.5f;
    public int normalEnemiesPerWave = 5;
    public int hardEnemiesPerWave = 10;

    // Player skill scaling
    public float normalSkillLevel = 1f;
    public float hardSkillLevel = 1.5f;

    void Start()
    {
        navMeshSurface = FindObjectOfType<NavMeshSurface>();

        // Instantiate the tower prefab at the desired position
        tower = Instantiate(towerPrefab, new Vector3(128f, 5.32f, 128f), Quaternion.identity);

        // Set up references and NavMesh rebaking
        InitializeTowerAndNavMesh();
        defenderPlacementManager.TogglePlacementMode(false);

        // Set the difficulty settings based on player selection
        SetupDifficulty();
    }

    private void InitializeTowerAndNavMesh()
    {
        MainTowerController towerController = tower.GetComponent<MainTowerController>();

        if (towerController != null && enemySpawner != null)
        {
            // Assign references as before
            towerController.pathManager = pathManager;
            towerController.enemySpawner = enemySpawner;
            towerController.terrain = terrain;
            towerController.projectilePrefab = projectilePrefab;
            enemySpawner.mainTowerController = towerController;

            // Set the tower in the PathManager
            pathManager.SetTower(tower.transform);

            // Set the camera target to follow the tower
            if (cameraController != null)
            {
                cameraController.SetTarget(tower.transform);
            }

            // Ensure TerrainGenerator is available
            if (terrainGenerator == null)
            {
                terrainGenerator = FindObjectOfType<TerrainGenerator>();
            }
            else
            {
                Debug.LogError("TerrainGenerator not found.");
            }
        }
        else
        {
            if (towerController == null)
                Debug.LogError("MainTowerController not found on the tower prefab.");
            if (enemySpawner == null)
                Debug.LogError("EnemySpawner reference is missing.");
        }
    }

    private IEnumerator DelayNavMeshRebake()
    {
        // Wait a short time to ensure everything is ready before rebaking
        yield return new WaitForSeconds(0f);

        terrainGenerator.ReBakeNavMesh();
        Debug.Log("NavMesh re-baked after delay.");
    }

    private void SetupDifficulty()
    {
        string difficulty = PlayerPrefs.GetString("GameDifficulty", "Normal");

        if (difficulty == "Normal")
        {
            SetupNormalDifficulty();
        }
        else if (difficulty == "Hard")
        {
            SetupHardDifficulty();
        }
    }

    private void SetupNormalDifficulty()
    {
        Debug.Log("Setting up Normal Difficulty");

        // Adjust parameters for normal difficulty
        enemySpawner.spawnInterval = normalSpawnInterval;
        enemySpawner.baseEnemiesPerWave = normalEnemiesPerWave;
        enemySpawner.SetPlayerSkillLevel(normalSkillLevel);  // Set the initial skill level for normal difficulty

        // Additional adjustments for normal difficulty if needed
    }

    private void SetupHardDifficulty()
    {
        Debug.Log("Setting up Hard Difficulty");

        // Adjust parameters for hard difficulty
        enemySpawner.spawnInterval = hardSpawnInterval;
        enemySpawner.baseEnemiesPerWave = hardEnemiesPerWave;
        enemySpawner.SetPlayerSkillLevel(hardSkillLevel);  // Set the initial skill level for hard difficulty

        // Hard difficulty modifications can include:
        // - Increased enemy health
        // - Faster enemy movement speed
        // - More spawn points or waves
        enemySpawner.SetEnemyDifficultyScale(1.5f);  // Increase enemy health/damage by 1.5 times
    }
}
