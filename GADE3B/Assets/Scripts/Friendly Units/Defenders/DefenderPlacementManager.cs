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
    public GameObject defenderPrefab; // Set this in the inspector
    public LayerMask terrainLayer; // Set the terrain layer in the inspector
    public Camera mainCamera; // Reference to the main camera
    public GoldManager gameManager; // Reference to the GameManager which controls the gold
    public TerrainGenerator terrainGenerator; // Reference to the TerrainGenerator to rebake NavMesh
    public PathManager pathManager; // Reference to the PathManager for predetermined positions
    public float placementThreshold = 2.0f; // Max distance from a valid position to allow placement
    public bool isPlacing = false; // Track whether we're in placement mode

    void Update()
    {
        if (isPlacing)
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                TryPlaceDefender();
            }
        }
    }

    public void TogglePlacementMode(bool toggle)
    {
        isPlacing = toggle;
        Time.timeScale = isPlacing ? 0.1f : 1.0f; // Slow down time when in placement mode
    }

    private void TryPlaceDefender()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            Vector3 spawnPosition = hit.point;

            // Check if the spawn position is near any valid defender positions
            if (IsValidDefenderPosition(spawnPosition))
            {
                PlaceDefenderAtPosition(spawnPosition);
            }
            else
            {
                Debug.LogError("Invalid defender placement position. Must be near a predetermined location.");
            }
        }
        else
        {
            Debug.LogError("Raycast did not hit the terrain layer. Check layer settings.");
        }
    }

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

        return false; // Not close enough to a valid defender position
    }

    private void PlaceDefenderAtPosition(Vector3 spawnPosition)
    {
        spawnPosition.y += 0.5f; // Adjust height if necessary
        GameObject defender = Instantiate(defenderPrefab, spawnPosition, Quaternion.identity);

        // Spend gold to confirm placement
        if (gameManager.SpendGold(5))
        {
            // Add NavMeshObstacle if it doesn't already exist
            if (!defender.GetComponent<NavMeshObstacle>())
            {
                NavMeshObstacle navObstacle = defender.AddComponent<NavMeshObstacle>();
                navObstacle.carving = true; // Enable carving for dynamic obstacle avoidance
            }

            // Notify the PathManager about the new defender position
            pathManager.AddDefenderPosition(defender.transform.position);

            // Trigger NavMesh rebake after defender placement
            if (terrainGenerator != null)
            {
                terrainGenerator.ReBakeNavMesh();  // Re-bake the NavMesh to include the defender
                Debug.Log("NavMesh re-baked after defender placement.");
            }
            else
            {
                Debug.LogError("TerrainGenerator reference is missing.");
            }

            // Allow placement of more defenders if there is enough gold
            isPlacing = true; // Keep placement mode active
            Time.timeScale = 0.1f; // Keep the slowed-down time active for more placements
        }
        else
        {
            Debug.LogError("Not enough gold to place the defender.");
        }
    }
}
