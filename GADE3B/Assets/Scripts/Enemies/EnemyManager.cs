using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyController> activeEnemies = new List<EnemyController>(); // List to track active enemies

    // Reference to EnemySpawner to notify when enemies die
    public EnemySpawner enemySpawner;

    private void Start()
    {
        if (enemySpawner == null)
        {
            Debug.LogError("EnemySpawner reference not set in EnemyManager.");
        }
    }

    private void Update()
    {
        // Monitor enemies and remove dead ones from the active list
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i].IsDead())
            {
                RemoveEnemy(activeEnemies[i]);
            }
        }
    }

    public void AddEnemy(EnemyController enemy)
    {
        if (!activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
        }
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);

            // Notify the EnemySpawner that an enemy has been removed (died)
            enemySpawner.OnEnemyDeath();
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
