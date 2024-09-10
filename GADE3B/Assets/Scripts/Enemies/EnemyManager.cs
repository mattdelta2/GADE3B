using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemyPrefabs; // List to hold different enemy prefabs
    private List<EnemyController> activeEnemies = new List<EnemyController>(); // List to track active enemies

    public Transform tower; // Reference to the main tower

    private NavMeshAgent agent; // NavMeshAgent to handle movement and pathfinding

    // For tracking if the enemy is stuck
    private Vector3 lastPosition;
    private float stuckTimer = 0f;
    private float stuckThreshold = 2f; // Time (in seconds) before being considered stuck

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (tower == null)
        {
            Debug.LogError("Tower reference is not set.");
        }

        // Set the initial destination to the tower
        SetDestination(tower);

        // Initialize lastPosition to the current position
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (agent == null) return;

        // Check if the enemy has moved
        if (Vector3.Distance(transform.position, lastPosition) < 0.1f)
        {
            // If the enemy hasn't moved, increase the stuck timer
            stuckTimer += Time.deltaTime;

            // Check if the enemy has been stuck for too long
            if (stuckTimer >= stuckThreshold)
            {
                Debug.Log("Enemy is stuck and will be destroyed.");
                DestroyEnemy(); // Call the method to destroy the enemy
            }
        }
        else
        {
            // Reset the timer if the enemy has moved
            stuckTimer = 0f;
        }

        // Update lastPosition to the current position
        lastPosition = transform.position;
    }

    private void SetDestination(Transform target)
    {
        if (agent != null && target != null)
        {
            agent.SetDestination(tower.position);
        }
    }

    private void DestroyEnemy()
    {
        // Add any logic for what should happen before the enemy is destroyed, if needed
        Destroy(gameObject);
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
