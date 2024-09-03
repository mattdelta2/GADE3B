using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject towerPrefab;
    public PathManager pathManager;
    public EnemySpawner enemySpawner;
    public Terrain terrain;
    public GameObject projectilePrefab;

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
        }
        else
        {
            Debug.LogError("MainTowerController component not found on the tower prefab.");
        }
    }
}
