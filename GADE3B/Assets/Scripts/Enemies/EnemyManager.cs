using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // List to hold different enemy prefabs
    private List<EnemyController> activeEnemies = new List<EnemyController>(); // List to track active enemies

    public Terrain terrain; // Reference to the terrain

    void Start()
    {
        if (terrain == null)
        {
            terrain = Terrain.activeTerrain;
            if (terrain == null)
            {
                Debug.LogWarning("Terrain not assigned and no terrain found.");
            }
        }
    }

    // Method to spawn an enemy at a given position with a specified path
    public void SpawnEnemy(Vector3 spawnPosition, List<Vector3> path)
    {
        if (enemyPrefabs.Count == 0)
        {
            Debug.LogError("No enemy prefabs assigned.");
            return;
        }

        // Choose a random enemy type
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject enemyInstance = Instantiate(enemyPrefabs[randomIndex], spawnPosition, Quaternion.identity);

        EnemyController enemyController = enemyInstance.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.SetPath(path);
            enemyController.SetTerrain(terrain);
            activeEnemies.Add(enemyController);
        }
        else
        {
            Debug.LogError("Spawned enemy does not have an EnemyController script.");
        }
    }

    // Method to remove an enemy from the active list when it's destroyed
    public void RemoveEnemy(EnemyController enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }

    // Method to clear all enemies (if needed)
    public void ClearAllEnemies()
    {
        foreach (var enemy in activeEnemies)
        {
            Destroy(enemy.gameObject);
        }
        activeEnemies.Clear();
    }
}
