using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{/*
    public float moveSpeed = 5f;  // Speed at which the enemy moves
    public Terrain terrain;  // Public reference to the Terrain component
    private List<Vector3> path;
    private int currentPointIndex = 0;

    void Start()
    {
        if (terrain == null)
        {
            Debug.LogWarning("Terrain not assigned. Please assign it in the Inspector or programmatically.");
        }
    }

    void Update()
    {
        if (path != null && path.Count > 0)
        {
            MoveAlongPath();
        }
    }

    public void SetPath(List<Vector3> newPath)
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain is not assigned. Cannot set path.");
            return;
        }

        path = newPath;
        if (path.Count > 0)
        {
            transform.position = GetTerrainHeightAtPosition(path[0]);
            currentPointIndex = 0; // Reset the path index
            Debug.Log("Path set with points:");
            foreach (Vector3 point in path)
            {
                Debug.Log(point);
            }
        }
        else
        {
            Debug.LogWarning("Path is empty. No movement will occur.");
        }
    }

    void MoveAlongPath()
    {

        if (currentPointIndex >= path.Count) return;

        Vector3 target = path[currentPointIndex];
        Vector3 direction = (target - transform.position).normalized;
        float step = moveSpeed * Time.deltaTime;

        // Move towards the target point
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // Update the height to follow the terrain
        transform.position = GetTerrainHeightAtPosition(transform.position);

        // Check if the enemy has reached the target point
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            Debug.Log($"Reached point {currentPointIndex}: {target}");

            currentPointIndex++;
            if (currentPointIndex < path.Count)
            {
                // Update the height at the next point
                transform.position = GetTerrainHeightAtPosition(path[currentPointIndex]);
            }
            else
            {
                Debug.Log("Path complete.");
            }
        }
    }

    Vector3 GetTerrainHeightAtPosition(Vector3 position)
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain is not assigned. Cannot get terrain height.");
            return position;
        }

        float terrainHeight = terrain.SampleHeight(position);
        return new Vector3(position.x, terrainHeight, position.z);
    }





    public float moveSpeed = 5f;  // Speed at which the enemy moves
    private Terrain terrain;  // Private reference to the Terrain component
    private List<Vector3> path;
    private int currentPointIndex = 0;


    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        // Check if any code here changes position
        Debug.Log($"Initial Position: {initialPosition}");
    }

    void Start()
    {
        // Ensure terrain is assigned before starting
        if (terrain == null)
        {
            terrain = Terrain.activeTerrain;
            if (terrain == null)
            {
                Debug.LogWarning("Terrain not assigned and no terrain found.");
            }
        }
    }

    void Update()
    {
        if (path != null && path.Count > 0)
        {
            MoveAlongPath();
        }
    }

    public void SetPath(List<Vector3> newPath)
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain is not assigned.");
            return;
        }

        path = newPath;
        if (path.Count > 0)
        {
            transform.position = GetTerrainHeightAtPosition(path[0]);
        }
    }

    public void SetTerrain(Terrain terrainReference)
    {
        terrain = terrainReference;
    }

    void MoveAlongPath()
    {
        if (currentPointIndex >= path.Count) return;

        Vector3 target = path[currentPointIndex];
        Vector3 direction = (target - transform.position).normalized;
        float step = moveSpeed * Time.deltaTime;

        // Move towards the target point
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // Update the height to follow the terrain
        transform.position = GetTerrainHeightAtPosition(transform.position);

        // Check if the enemy has reached the target point
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            currentPointIndex++;
            if (currentPointIndex < path.Count)
            {
                // Update the height at the next point
                transform.position = GetTerrainHeightAtPosition(path[currentPointIndex]);
            }
        }
    }

    Vector3 GetTerrainHeightAtPosition(Vector3 position)
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain is not assigned.");
            return position;
        }

        float terrainHeight = terrain.SampleHeight(position);
        return new Vector3(position.x, terrainHeight, position.z);
    }




    public float moveSpeed = 5f;  // Speed at which the enemy moves
    private Terrain terrain;  // Private reference to the Terrain component
    private List<Vector3> path;
    private int currentPointIndex = 0;

    private void Start()
    {
        // Ensure terrain is assigned before starting
        if (terrain == null)
        {
            terrain = Terrain.activeTerrain;
            if (terrain == null)
            {
                Debug.LogWarning("Terrain not assigned and no terrain found.");
            }
        }

        Debug.Log($"Initial Position: {transform.position}");
    }

    private void Update()
    {
        if (path != null && path.Count > 0)
        {
            MoveAlongPath();
        }
    }

    public void SetPath(List<Vector3> newPath)
    {
        Debug.Log("Path set:");
        foreach (var point in path)
        {
            Debug.Log($"Path Point: {point}");
        }
        if (terrain == null)
        {
            Debug.LogError("Terrain is not assigned.");
            return;
        }

        path = newPath;
        Debug.Log("Path set:");
        foreach (var point in path)
        {
            Debug.Log($"Path Point: {point}");
        }

        if (path.Count > 0)
        {
            transform.position = GetTerrainHeightAtPosition(path[0]);
            Debug.Log($"Initial Position adjusted to: {transform.position}");
        }
    }

    public void SetTerrain(Terrain terrainReference)
    {
        terrain = terrainReference;
    }

    private void MoveAlongPath()
    {
        if (currentPointIndex >= path.Count) return;

        Vector3 target = path[currentPointIndex];
        Vector3 direction = (target - transform.position).normalized;
        float step = moveSpeed * Time.deltaTime;

        // Move towards the target point
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // Log movement
        Debug.Log($"Moving towards: {target}, Current Position: {transform.position}");

        // Update the height to follow the terrain
        transform.position = GetTerrainHeightAtPosition(transform.position);

        // Check if the enemy has reached the target point
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            Debug.Log($"Reached point: {target}");
            currentPointIndex++;
            if (currentPointIndex < path.Count)
            {
                // Update the height at the next point
                transform.position = GetTerrainHeightAtPosition(path[currentPointIndex]);
                Debug.Log($"Next point: {path[currentPointIndex]}");
            }
        }
    }

    private Vector3 GetTerrainHeightAtPosition(Vector3 position)
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain is not assigned.");
            return position;
        }

        float terrainHeight = terrain.SampleHeight(position);
        Vector3 adjustedPosition = new Vector3(position.x, terrainHeight, position.z);
        Debug.Log($"Adjusted Position: {adjustedPosition} for Input Position: {position}");
        return adjustedPosition;
    }*/


    public float health = 20f; // Initial health of the enemy
    public float maxHealth = 20f; // Max health of the enemy
    public float damageAmount = 10f; // Damage amount the enemy will deal
    public float range = 10f; // Range within which the enemy can attack
    public float shootingInterval = 2f; // Time between attacks
    public GameObject projectilePrefab; // Prefab of the projectile to shoot
    public GameObject healthBarPrefab; // Health bar prefab
    private GameObject healthBar; // Instance of the health bar
    private Slider healthBarSlider; // Reference to the slider component

    private float shootingTimer = 0f;
    public Transform tower; // Reference to the main tower
    private Transform currentTarget; // Current target (either tower or a defender)
    private NavMeshAgent agent; // NavMeshAgent to handle movement and pathfinding
    public Transform shootProjectile; // Where the projectiles will spawn from

    private void Start()
    {
        shootProjectile = transform.Find("shootProjectile");

        // Ensure the tower is assigned
        if (tower == null)
        {
            Debug.LogError("Tower reference is not set.");
        }

        // Get or add a NavMeshAgent component to the enemy
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
        }

        // Instantiate the health bar and set its max health
        healthBar = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform);
        healthBarSlider = healthBar.GetComponentInChildren<Slider>();
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = health;

        // Set initial destination to the tower
        SetDestination(tower);
    }

    private void Update()
    {
        // Null check for the agent
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing on this enemy.");
            return; // Exit early if the agent is null
        }

        // Handle if the current target has been destroyed or is null
        if (currentTarget == null || currentTarget.gameObject == null)
        {
            Debug.LogWarning("Current target has been destroyed or is null. Finding a new target.");
            currentTarget = FindNewTarget();

            // If no new target is found, stop the enemy or handle fallback behavior
            if (currentTarget == null)
            {
                Debug.LogWarning("No valid target found. Stopping enemy.");
                agent.isStopped = true; // Optional: stop the enemy if no target is found
                return;
            }
        }

        // Ensure destination is set and check if the agent is moving
        if (agent.hasPath && currentTarget != null)
        {
            Debug.Log("Agent moving towards: " + currentTarget.name + " | Remaining Distance: " + agent.remainingDistance);
        }

        // Handle shooting logic
        shootingTimer += Time.deltaTime;
        if (shootingTimer >= shootingInterval)
        {
            ShootAtTarget();
            shootingTimer = 0f; // Reset the shooting timer
        }

        // Continuously search for defenders to prioritize them
        FindClosestDefender();

        // Update health bar position to stay above the enemy
        if (healthBar != null)
        {
            healthBar.transform.position = transform.position + Vector3.up * 2;
        }

        // Check if there's a defender to attack or move toward the tower
        if (currentTarget != null && agent.remainingDistance > 0.5f)
        {
            SetDestination(currentTarget); // Attack defender if it's in range, else move towards the tower
        }
    }

    // Set the destination to a target (either defender or tower)
    private void SetDestination(Transform target)
    {
        if (agent != null && target != null && target.gameObject != null) // Ensure the target hasn't been destroyed
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // Only set the destination if it's far enough
            if (distanceToTarget > 1f) // Set a minimum distance to avoid constant recalculation
            {
                agent.SetDestination(target.position);
                Debug.Log("Setting destination to: " + target.name + " at position: " + target.position);
            }
            else
            {
                Debug.Log("Target is too close. Not setting a new destination.");
            }
        }
        else
        {
            Debug.LogError("NavMeshAgent or Target is null or destroyed.");
        }
    }

    // Shoot a projectile at the current target
    private void ShootAtTarget()
    {
        if (currentTarget != null && projectilePrefab != null)
        {
            // Check if the enemy is within shooting range of the target
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget <= range) // Only shoot if within range
            {
                // Log the shoot position to ensure it's correct
                Debug.Log("Shooting projectile from: " + shootProjectile.position);

                // Create the projectile and shoot it at the target
                GameObject projectile = Instantiate(projectilePrefab, shootProjectile.position, Quaternion.identity);
                Debug.Log("Projectile instantiated at position: " + shootProjectile.position);

                ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
                if (projectileController != null)
                {
                    projectileController.SetTarget(currentTarget); // Shoot at the current target
                }

                // Check if the target is a defender
                DefenderController defender = currentTarget.GetComponent<DefenderController>();
                if (defender != null)
                {
                    defender.TakeDamage(damageAmount); // Apply damage to the defender
                }
                else
                {
                    // If it's not a defender, assume it's the tower
                    MainTowerController towerController = currentTarget.GetComponent<MainTowerController>();
                    if (towerController != null)
                    {
                        towerController.TakeDamage(damageAmount); // Apply damage to the tower
                    }
                }
            }
            else
            {
                Debug.Log("Enemy is not within shooting range.");
            }
        }
    }

    // Find the closest defender to attack
    private void FindClosestDefender()
    {
        Collider[] hitDefenders = Physics.OverlapSphere(transform.position, range);
        float closestDistance = Mathf.Infinity;
        Transform closestDefender = null;

        foreach (Collider defenderCollider in hitDefenders)
        {
            if (defenderCollider.CompareTag("Defender"))
            {
                Transform defenderTransform = defenderCollider.transform;

                // Skip destroyed defenders
                if (defenderTransform == null || defenderTransform.gameObject == null)
                {
                    continue;  // Skip null or destroyed objects
                }

                float distanceToDefender = Vector3.Distance(transform.position, defenderTransform.position);
                if (distanceToDefender < closestDistance)
                {
                    closestDistance = distanceToDefender;
                    closestDefender = defenderTransform;
                }
            }
        }

        if (closestDefender != null)
        {
            currentTarget = closestDefender;
        }
        else
        {
            currentTarget = tower; // Target the tower if no defenders are found
        }
    }

    // Find a new target if the current one is destroyed
    private Transform FindNewTarget()
    {
        // First, try to find the closest defender
        FindClosestDefender();

        // If no defenders are found, return the tower
        return currentTarget != null ? currentTarget : tower;
    }

    // Take damage method
    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBarSlider.value = health; // Update the health bar when taking damage
        if (health <= 0)
        {
            Die();
        }
    }

    // Handle the enemy's death
    private void Die()
    {
        Destroy(healthBar); // Destroy the health bar when the enemy dies
        Destroy(gameObject); // Destroy the enemy
    }

    // Helper method to check if the enemy is dead
    public bool IsDead()
    {
        return health <= 0;
    }

    // Method to recalculate path (for when the NavMesh is updated)
    public void RecalculatePath()
    {
        // Check if the NavMeshAgent and currentTarget are valid
        if (agent != null)
        {
            // If the currentTarget is null or destroyed, find a new valid target
            if (currentTarget == null || currentTarget.gameObject == null)
            {
                Debug.LogWarning("Current target is null or destroyed. Finding a new target.");
                currentTarget = FindNewTarget(); // Use your existing FindNewTarget method
            }

            // If a valid target is found, set the agent's destination
            if (currentTarget != null)
            {
                agent.SetDestination(currentTarget.position);
                Debug.Log("Enemy recalculated path to target: " + currentTarget.name);
            }
            else
            {
                Debug.LogWarning("No valid target found. Stopping enemy.");
                agent.isStopped = true; // Stop the enemy if no target exists
            }
        }
    }
}



