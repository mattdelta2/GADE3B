using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // List to hold different enemy prefabs
    private List<EnemyController> activeEnemies = new List<EnemyController>(); // List to track active enemies

    public Transform tower; // Reference to the main tower

    void Start()
    {
        if (tower == null)
        {
            Debug.LogError("Tower reference is not assigned.");
        }
    }

    // Method to spawn an enemy at a given position

/*
    code to add waves and increase diffuculty to game!!!!!!


    
    public int waveNumber = 1;

    public void SpawnEnemy(Vector3 spawnPosition)
    {
        if (enemyPrefabs.Count == 0)
        {
            Debug.LogError("No enemy prefabs assigned.");
            return;
        }

        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        GameObject enemyInstance = Instantiate(enemyPrefabs[randomIndex], spawnPosition, Quaternion.identity);

        EnemyController enemyController = enemyInstance.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.tower = tower;
            enemyController.health += waveNumber * 10; // Increase health per wave
            enemyController.damageAmount += waveNumber * 2; // Increase damage per wave

            NavMeshAgent agent = enemyInstance.GetComponent<NavMeshAgent>();
            if (agent != null && tower != null)
            {
                agent.SetDestination(tower.position);
            }

            activeEnemies.Add(enemyController);
        }
    }
    */
    public void SpawnEnemy(Vector3 spawnPosition)
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
            // Assign the tower to the enemy
            enemyController.tower = tower;

            // Get the NavMeshAgent component
            NavMeshAgent agent = enemyInstance.GetComponent<NavMeshAgent>();
            if (agent != null && tower != null)
            {
                // Set the destination to the tower
                agent.SetDestination(tower.position);
            }

            // Add the enemy to the list of active enemies
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
        if (enemy != null && activeEnemies.Contains(enemy))
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
