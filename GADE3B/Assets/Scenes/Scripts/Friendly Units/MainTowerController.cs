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


    public GameObject mainTowerPrefab;
    public GameObject PlacedTower => placedTower;
    public PathManager pathManager;  // Reference to PathManager
    public EnemySpawner enemySpawner; // Reference to EnemySpawner
    public Terrain terrain; // Reference to the terrain

    private GameObject placedTower;  // Reference to the placed tower

    void Start()
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain is not assigned.");
            return;
        }

        Vector3 terrainCenter = new Vector3(terrain.terrainData.size.x / 2, 0, terrain.terrainData.size.z / 2);
        float terrainHeight = terrain.SampleHeight(terrainCenter);
        Vector3 centerPosition = new Vector3(terrainCenter.x, terrainHeight, terrainCenter.z);

        if (placedTower == null)
        {
            placedTower = Instantiate(mainTowerPrefab, centerPosition, Quaternion.identity);
            Debug.Log($"Main Tower placed at: {centerPosition}");

            if (pathManager != null && placedTower != null)
            {
                pathManager.SetTower(placedTower.transform);
                Debug.Log("PathManager notified to generate paths.");
            }
            else
            {
                Debug.LogError("PathManager or placedTower is null.");
            }

            if (enemySpawner != null)
            {
                enemySpawner.StartSpawning();
                Debug.Log("EnemySpawner notified to start spawning.");
            }
            else
            {
                Debug.LogError("EnemySpawner is null.");
            }
        }
        else
        {
            Debug.LogWarning("Main Tower is already placed.");
        }
    }




}
