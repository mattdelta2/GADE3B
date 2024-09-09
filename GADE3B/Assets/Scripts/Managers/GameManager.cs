using UnityEngine;

public class GameManager : MonoBehaviour
{/*
    public GameObject towerPrefab;
    public PathManager pathManager;
    public EnemySpawner enemySpawner;
    public Terrain terrain;
    public GameObject projectilePrefab;
    public FollowCamera cameraController; // Reference to your CameraController script
    public TerrainGenerator terrainGenerator;  // Reference to the TerrainGenerator

    void Start()
    {
        // Instantiate the tower prefab
        GameObject tower = Instantiate(towerPrefab, new Vector3(128f, 5.32f, 128f), Quaternion.identity);

        // Get the MainTowerController component
        MainTowerController towerController = tower.GetComponent<MainTowerController>();

        // Set references
        if (towerController != null)
        {
            towerController.pathManager = pathManager;
            towerController.enemySpawner = enemySpawner;
            towerController.terrain = terrain;
            towerController.projectilePrefab = projectilePrefab;

            pathManager.SetTower(tower.transform);

            Debug.Log("PathManager assigned: " + (pathManager != null));
            Debug.Log("EnemySpawner assigned: " + (enemySpawner != null));
            Debug.Log("Terrain assigned: " + (terrain != null));
            Debug.Log("ProjectilePrefab assigned: " + (projectilePrefab != null));

            // Set the camera target to the tower's transform
            if (cameraController != null)
            {
                cameraController.SetTarget(tower.transform);
                Debug.Log("Camera target set to the tower.");
            }
            else
            {
                Debug.LogError("CameraController reference not set in GameManager.");
            }

            // After placing the tower, re-bake the NavMesh to include the tower
            if (terrainGenerator != null)
            {
                terrainGenerator.ReBakeNavMesh();
                Debug.Log("NavMesh re-baked to include the tower.");
            }
            else
            {
                Debug.LogError("TerrainGenerator reference is missing in GameManager.");
            }

            // Set the tower reference in EnemySpawner
            if (enemySpawner != null)
            {
                enemySpawner.SetMainTowerController(towerController);
            }
            else
            {
                Debug.LogError("EnemySpawner reference is missing in GameManager.");
            }
        }
        else
        {
            Debug.LogError("MainTowerController component not found on the tower prefab.");
        }
    }*/




    public GameObject towerPrefab;
    public PathManager pathManager;
    public EnemySpawner enemySpawner;
    public Terrain terrain;
    public GameObject projectilePrefab;
    public FollowCamera cameraController;
    public TerrainGenerator terrainGenerator;

    private GameObject tower; // Store reference to the instantiated tower

    void Start()
    {
        // Instantiate the tower prefab at the desired position
        tower = Instantiate(towerPrefab, new Vector3(128f, 5.32f, 128f), Quaternion.identity);

        // Get the MainTowerController component from the instantiated tower
        MainTowerController towerController = tower.GetComponent<MainTowerController>();

        // Ensure the towerController and enemySpawner exist
        if (towerController != null && enemySpawner != null)
        {
            // Set references in the MainTowerController
            towerController.pathManager = pathManager;
            towerController.enemySpawner = enemySpawner;
            towerController.terrain = terrain;
            towerController.projectilePrefab = projectilePrefab;
            towerController.terrainGenerator = terrainGenerator;

            // Assign MainTowerController to EnemySpawner
            enemySpawner.mainTowerController = towerController;
            Debug.Log("MainTowerController assigned to EnemySpawner.");

            // Set the tower in the PathManager
            pathManager.SetTower(tower.transform);

            // Set the camera target to follow the tower
            if (cameraController != null)
            {
                cameraController.SetTarget(tower.transform);
                Debug.Log("Camera target set to the tower.");
            }

            // Re-bake the NavMesh to account for the tower placement
            if (terrainGenerator != null)
            {
                terrainGenerator.ReBakeNavMesh();
                Debug.Log("NavMesh re-baked to include the tower.");
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
}
