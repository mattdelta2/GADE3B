using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TowerPlacementController : MonoBehaviour
{
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
                // Get the terrain height
                Vector3 terrainPosition = new Vector3(worldPosition.x, terrain.SampleHeight(worldPosition), worldPosition.z);

                // Ensure tower is placed on the NavMesh
                NavMeshHit hit;
                if (NavMesh.SamplePosition(terrainPosition, out hit, 5f, NavMesh.AllAreas))
                {
                    placedTower = Instantiate(mainTowerPrefab, hit.position, Quaternion.identity);
                    canPlaceMainTower = false;

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
                    Debug.LogError("Cannot place tower, invalid NavMesh position.");
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
}
