using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefenderPlacementManager : MonoBehaviour
{
    public GameObject[] defenderPrefabs;    // Array of all defender prefabs
    public LayerMask terrainLayer;          // Set the terrain layer in the inspector
    public Camera mainCamera;               // Reference to the main camera
    public GoldManager gameManager;         // Reference to the GameManager which controls the gold
    public NavMeshSurface navMeshSurface;   // Reference to the NavMeshSurface used for full rebaking
    public PathManager pathManager;         // Reference to the PathManager for predetermined positions
    public float placementThreshold = 2.0f; // Max distance from a valid position to allow placement
    public EnemySpawner enemySpawner;       // Reference to EnemySpawner to notify about placed defenders

    private GameObject selectedDefenderPrefab; // The currently selected defender prefab
    public bool isPlacing = false;          // Track whether we're in placement mode
    public GameObject defenderSelectionUI;  // Reference to the UI containing the defender selection buttons

    private void Update()
    {
        // If we're in placement mode and a defender has been selected
        if (isPlacing && selectedDefenderPrefab != null)
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                TryPlaceDefender(); // Try to place the selected defender
            }
        }
    }

    // Method to enter/exit placement mode
    public void TogglePlacementMode(bool toggle)
    {
        isPlacing = toggle;

        // Show the defender selection UI only when in placement mode
        if (defenderSelectionUI != null)
        {
            defenderSelectionUI.SetActive(isPlacing); // Show or hide the selection UI
        }

        Time.timeScale = isPlacing ? 0.1f : 1.0f; // Slow down time when in placement mode

        if (!isPlacing)
        {
            selectedDefenderPrefab = null; // Clear the selected defender when exiting placement mode
        }
    }

    // Method to select the defender (called by the UI buttons)
    public void SelectDefender(int defenderIndex)
    {
        if (defenderPrefabs == null || defenderPrefabs.Length == 0)
        {
            Debug.LogError("Defender prefabs array is not assigned or empty.");
            return;
        }

        if (defenderIndex >= 0 && defenderIndex < defenderPrefabs.Length)
        {
            selectedDefenderPrefab = defenderPrefabs[defenderIndex];
            Debug.Log("Defender selected: " + selectedDefenderPrefab.name);
        }
        else
        {
            Debug.LogError("Invalid defender index: " + defenderIndex);
        }
    }

    // Method to try placing the selected defender
    private void TryPlaceDefender()
    {
        if (selectedDefenderPrefab == null)
        {
            Debug.LogError("No defender selected.");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            Vector3 spawnPosition = hit.point;

            // Check if the spawn position is valid
            if (IsValidDefenderPosition(spawnPosition))
            {
                PlaceDefenderAtPosition(spawnPosition);
            }
            else
            {
                Debug.LogError("Invalid defender placement position.");
            }
        }
        else
        {
            Debug.LogError("Raycast did not hit the terrain layer.");
        }
    }

    // Helper to check if the defender's position is valid
    private bool IsValidDefenderPosition(Vector3 position)
    {
        if (pathManager == null)
        {
            Debug.LogError("PathManager reference is missing.");
            return false;
        }

        foreach (Vector3 defenderPosition in pathManager.defenderPositions)
        {
            if (Vector3.Distance(position, defenderPosition) <= placementThreshold)
            {
                return true;
            }
        }

        return false;
    }

    // Place the selected defender at the given position
    private void PlaceDefenderAtPosition(Vector3 spawnPosition)
    {
        if (gameManager.SpendGold(5)) // Deduct gold if the player has enough
        {
            spawnPosition.y += 0.5f; // Adjust height

            GameObject defender = Instantiate(selectedDefenderPrefab, spawnPosition, Quaternion.identity);

            // Add NavMeshObstacle to the placed defender
            if (!defender.GetComponent<NavMeshObstacle>())
            {
                NavMeshObstacle navObstacle = defender.AddComponent<NavMeshObstacle>();
                navObstacle.carving = true;
            }

            // Notify the PathManager of the new defender position
            pathManager.AddDefenderPosition(defender.transform.position);

            // Notify the EnemySpawner to update the defender types based on the placed defender
            NotifyEnemySpawner(defender);

            // Rebuild the NavMesh
            StartCoroutine(RebakeNavMeshAndRecalculatePaths());

            // Allow placement mode to stay active for more placements
            selectedDefenderPrefab = null; // Clear the selection for the next defender
        }
        else
        {
            Debug.LogError("Not enough gold to place the defender.");
        }
    }

    private void NotifyEnemySpawner(GameObject defender)
    {
        // Check defender type and notify the EnemySpawner about the placed defender type
        DefenderController defenderController = defender.GetComponent<DefenderController>();
        if (defenderController != null)
        {
            string defenderType = defenderController.defenderType; // Assuming defenders have a defenderType field
            enemySpawner.AddDefenderType(defenderType);
        }
    }

    private IEnumerator RebakeNavMeshAndRecalculatePaths()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh(); // Rebake the entire NavMesh
        }

        yield return new WaitForSecondsRealtime(0.1f); // Small delay

        RecalculateEnemyPaths();
    }

    private void RecalculateEnemyPaths()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.RecalculatePath(); // Update enemy pathfinding
            }
        }
    }
}
