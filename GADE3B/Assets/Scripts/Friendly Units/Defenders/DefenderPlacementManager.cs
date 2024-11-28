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
    public bool isUpgrading = false;        // Track whether we're in upgrade mode
    public GameObject defenderSelectionUI;  // Reference to the UI containing the defender selection buttons

    private void Update()
    {
        if (isPlacing && selectedDefenderPrefab != null)
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                TryPlaceDefender();
            }
        }

        if (isUpgrading)
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                TryUpgradeDefender();
            }
        }
    }

    // Toggle placement mode
    public void TogglePlacementMode(bool toggle)
    {
        isPlacing = toggle;
        isUpgrading = false;

        if (defenderSelectionUI != null)
        {
            defenderSelectionUI.SetActive(isPlacing);
        }

        Time.timeScale = isPlacing ? 0.1f : 1.0f;

        if (!isPlacing)
        {
            selectedDefenderPrefab = null;
        }
    }

    // Toggle upgrade mode
    public void ToggleUpgradeMode(bool toggle)
    {
        isUpgrading = toggle;
        isPlacing = false;

        if (defenderSelectionUI != null)
        {
            defenderSelectionUI.SetActive(false);
        }

        Time.timeScale = isUpgrading ? 0.1f : 1.0f;
    }

    // Select defender for placement
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

            if (IsValidDefenderPosition(spawnPosition))
            {
                PlaceDefenderAtPosition(spawnPosition);
            }
            else
            {
                Debug.LogError("Invalid defender placement position.");
            }
        }
    }

    private void TryUpgradeDefender()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            DefenderController defender = hit.collider.GetComponent<DefenderController>();

            if (defender != null)
            {
                if (gameManager.SpendGold(10)) // Upgrade cost
                {
                    defender.UpgradeDefender();
                }
                else
                {
                    Debug.LogError("Not enough gold to upgrade the defender.");
                }
            }
            else
            {
                Debug.LogError("No defender found to upgrade.");
            }
        }
    }

    private bool IsValidDefenderPosition(Vector3 position)
    {
        foreach (Vector3 defenderPosition in pathManager.defenderPositions)
        {
            if (Vector3.Distance(position, defenderPosition) <= placementThreshold)
            {
                return true;
            }
        }

        return false;
    }

    private void PlaceDefenderAtPosition(Vector3 spawnPosition)
    {
        if (gameManager.SpendGold(5)) // Deduct gold
        {
            spawnPosition.y += 0.5f;

            GameObject defender = Instantiate(selectedDefenderPrefab, spawnPosition, Quaternion.identity);

            if (!defender.GetComponent<NavMeshObstacle>())
            {
                NavMeshObstacle navObstacle = defender.AddComponent<NavMeshObstacle>();
                navObstacle.carving = true;
            }

            pathManager.AddDefenderPosition(defender.transform.position);
            NotifyEnemySpawner(defender);

            StartCoroutine(RebakeNavMeshAndRecalculatePaths());

            selectedDefenderPrefab = null;
        }
    }

    private void NotifyEnemySpawner(GameObject defender)
    {
        DefenderController defenderController = defender.GetComponent<DefenderController>();
        if (defenderController != null)
        {
            enemySpawner.AddDefenderType(defenderController.defenderType);
        }
    }

    private IEnumerator RebakeNavMeshAndRecalculatePaths()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }

        yield return new WaitForSecondsRealtime(0.1f);

        RecalculateEnemyPaths();
    }

    private void RecalculateEnemyPaths()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            enemy?.RecalculatePath();
        }
    }
}
