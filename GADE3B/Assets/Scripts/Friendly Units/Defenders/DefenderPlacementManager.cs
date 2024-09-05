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
    public bool isPlacing = false; // Track whether we're in placement mode

    private GameObject currentDefender;

    void Update()
    {
        if (isPlacing)
        {
            if (Input.GetMouseButtonDown(0) && currentDefender == null) // Left mouse button
            {

                StartPlacingDefender();

            }
            else if (Input.GetMouseButtonDown(1) && currentDefender != null) // Right mouse button to confirm placement
            {
                ConfirmPlacement();
            }

            if (currentDefender != null)
            {
                MoveDefenderToCursor();
            }
        }
    }

    public void TogglePlacementMode(bool toggle)
    {
        isPlacing = toggle;
        Time.timeScale = isPlacing ? 0.1f : 1.0f; // Slow down time when in placement mode

        if (!isPlacing && currentDefender != null)
        {
            // If exiting placement mode and cannot afford another defender, keep the last placed defender
            currentDefender = null; // This avoids deleting it but stops further interaction
        }
    }

    private void StartPlacingDefender()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            Vector3 spawnPosition = hit.point;
            spawnPosition.y += 0.5f; // Adjust height if necessary
            currentDefender = Instantiate(defenderPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void ConfirmPlacement()
    {
        if (currentDefender != null)
        {
            gameManager.SpendGold(5); // Spend gold to confirm placement

            // Add NavMeshObstacle to the defender to block enemy paths
            NavMeshObstacle navObstacle = currentDefender.AddComponent<NavMeshObstacle>();
            navObstacle.carving = true; // Enable carving for dynamic obstacle avoidance

            // Notify the PathManager about the new defender position
            PathManager pathManager = FindObjectOfType<PathManager>(); // Find the PathManager instance
            if (pathManager != null)
            {
                pathManager.AddDefenderPosition(currentDefender.transform.position);
            }
            else
            {
                Debug.LogError("PathManager not found.");
            }

            currentDefender = null; // Placement is confirmed
        }
    }

    private void MoveDefenderToCursor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            Vector3 newPosition = hit.point;

            // Get the exact height from the terrain at the hit point (if using terrain)
            // This ensures the defender stays on the terrain
            Terrain activeTerrain = Terrain.activeTerrain;
            if (activeTerrain != null)
            {
                float terrainHeight = activeTerrain.SampleHeight(newPosition);
                newPosition.y = terrainHeight; // Set defender's Y position to match terrain height
            }
            else
            {
                newPosition.y += 0.5f; // Adjust if not using Terrain system
            }

            currentDefender.transform.position = newPosition;
        }
        else
        {
            Debug.LogError("Raycast did not hit the terrain layer. Check layer settings.");
        }
    }
}
