using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefenderPlacementManager : MonoBehaviour
{/*
    public GameObject defenderPrefab; // Set this in the inspector
    public LayerMask terrainLayer; // Set the terrain layer in the inspector
    public Camera mainCamera; // Reference to the main camera
    public GoldManager goldManager; // Reference to the GoldManager
    public int defenderCost = 5; // Cost to place a defender

    public bool isPlacing = false; // Track whether we're in placement mode
    private GameObject currentDefender;

    void Update()
    {
        if (isPlacing && goldManager.currentGold >= defenderCost)
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                if (currentDefender == null)
                {
                    PlaceDefenderAtCursor();
                }
                else
                {
                    ConfirmPlacement();
                }
            }

            if (currentDefender != null)
            {
                MoveDefenderToCursor();
            }
        }
    }

    private void PlaceDefenderAtCursor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            if (currentDefender == null)
            {
                Vector3 spawnPosition = hit.point;
                spawnPosition.y += 0.5f; // Adjust height slightly above the terrain
                currentDefender = Instantiate(defenderPrefab, spawnPosition, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("Raycast did not hit the terrain layer.");
        }
    }

    private void MoveDefenderToCursor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Ensure we use the correct layer mask in the raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            Vector3 newPosition = hit.point;
            newPosition.y += 0.5f; // Adjust height if necessary
            currentDefender.transform.position = newPosition;
        }
        else
        {
            Debug.LogError("Raycast did not hit the terrain layer. Check layer settings.");
        }
    }

    public void TogglePlacementMode()
    {
        isPlacing = !isPlacing; // Toggle the placement mode
        if (!isPlacing && currentDefender != null)
        {
            Destroy(currentDefender); // Clean up if exiting placement mode without placing
        }
    }

    private void ConfirmPlacement()
    {
        // Finalize placement here, perhaps deduct gold etc.
        goldManager.SpendGold(defenderCost);
        currentDefender = null; // Ready for the next placement
    }
    */

    public GameObject[] defenderPrefabs;    // Array of all defender prefabs
    public LayerMask terrainLayer;          // Set the terrain layer in the inspector
    public Camera mainCamera;               // Reference to the main camera
    public GoldManager gameManager;         // Reference to the GameManager which controls the gold
    public NavMeshSurface navMeshSurface;   // Reference to the NavMeshSurface used for full rebaking
    public PathManager pathManager;         // Reference to the PathManager for predetermined positions
    public float placementThreshold = 2.0f; // Max distance from a valid position to allow placement

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
        Debug.Log("SelectDefender called with index: " + defenderIndex);

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
