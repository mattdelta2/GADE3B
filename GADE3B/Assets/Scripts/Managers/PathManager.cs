using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PathManager : MonoBehaviour
{
    public Terrain terrain;
    public GameObject defenderPlacementMarkerPrefab; // Prefab to indicate placement positions on the terrain
    public int numberOfSpawnPoints = 5;
    public float edgeMargin = 5f;
    public float minimumDistanceFromTower = 10f;
    public float defenderPlacementRadius = 5f;

    public List<Vector3> defenderPositions = new List<Vector3>(); // List to store defender positions
    private Transform tower;
    private Vector3[] spawnPoints;

    private void Start()
    {
        terrain = Terrain.activeTerrain;

        if (terrain == null)
        {
            Debug.LogError("Terrain not found in the scene.");
            return;
        }

        // Start waiting for NavMesh
        StartCoroutine(WaitForNavMesh());

        // Predefine at least 4 defender positions on the terrain
        PopulateDefenderPositions();

        // Visualize the defender positions
        VisualizeDefenderPositions();
    }

    private IEnumerator WaitForNavMesh()
    {
        TerrainGenerator terrainGenerator = FindObjectOfType<TerrainGenerator>();

        if (terrainGenerator == null)
        {
            Debug.LogError("TerrainGenerator not found in the scene.");
            yield break;
        }

        while (!terrainGenerator.IsNavMeshReady())
        {
            yield return null;
        }

        GeneratePathsForTower();
    }

    public void SetTower(Transform towerTransform)
    {
        tower = towerTransform;
        GeneratePathsForTower();
    }

    public void GeneratePathsForTower()
    {
        if (tower == null)
        {
            Debug.LogWarning("Tower is not set.");
            return;
        }

        spawnPoints = GenerateSpawnPoints();
        foreach (Vector3 spawn in spawnPoints)
        {
            List<Vector3> path = GeneratePath(spawn);

            // Visualize the path if valid
            if (path.Count > 0)
            {
                VisualizePath(path);
            }
        }
    }

    // Method to add a new defender position dynamically
    public void AddDefenderPosition(Vector3 position)
    {
        if (!defenderPositions.Contains(position) && !IsPointBlockedByDefender(position))
        {
            defenderPositions.Add(position);
            Debug.Log($"Added defender at position {position}");
            // Optionally visualize the new defender position
            if (defenderPlacementMarkerPrefab != null)
            {
                Instantiate(defenderPlacementMarkerPrefab, position, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log($"Position {position} is either blocked or already exists.");
        }
    }

    private void PopulateDefenderPositions()
    {
        defenderPositions.Add(new Vector3(110, terrain.SampleHeight(new Vector3(110, 0, 100)), 100));
        defenderPositions.Add(new Vector3(150, terrain.SampleHeight(new Vector3(150, 0, 150)), 150));
        defenderPositions.Add(new Vector3(220, terrain.SampleHeight(new Vector3(220, 0, 200)), 200));
        defenderPositions.Add(new Vector3(60, terrain.SampleHeight(new Vector3(60, 0, 50)), 50));
        defenderPositions.Add(new Vector3(94, terrain.SampleHeight(new Vector3(94, 0, 158)), 158));
        defenderPositions.Add(new Vector3(152, terrain.SampleHeight(new Vector3(152, 0, 95)), 95));
        defenderPositions.Add(new Vector3(50, terrain.SampleHeight(new Vector3(50, 0, 200)), 200));
        defenderPositions.Add(new Vector3(190, terrain.SampleHeight(new Vector3(190, 0, 50)), 50));

        Debug.Log("Defender positions populated.");
    }

    private void VisualizeDefenderPositions()
    {
        // Visualize each defender position with a marker
        foreach (Vector3 position in defenderPositions)
        {
            if (defenderPlacementMarkerPrefab != null)
            {
                Instantiate(defenderPlacementMarkerPrefab, position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Defender placement marker prefab is missing.");
            }
        }

        Debug.Log("Defender positions visualized.");
    }

    public Vector3[] GenerateSpawnPoints()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;

        Vector3[] points = new Vector3[numberOfSpawnPoints];
        int maxAttempts = 100;

        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            Vector3 spawnPoint = Vector3.zero;
            bool validPoint = false;
            int attempts = 0;

            while (!validPoint && attempts < maxAttempts)
            {
                attempts++;

                bool isEdgeX = UnityEngine.Random.value > 0.5f;
                bool isEdgeZ = UnityEngine.Random.value > 0.5f;

                float x = isEdgeX ? (UnityEngine.Random.value > 0.5f ? edgeMargin : terrainWidth - edgeMargin) : UnityEngine.Random.Range(edgeMargin, terrainWidth - edgeMargin);
                float z = isEdgeZ ? (UnityEngine.Random.value > 0.5f ? edgeMargin : terrainHeight - edgeMargin) : UnityEngine.Random.Range(edgeMargin, terrainHeight - edgeMargin);

                spawnPoint = new Vector3(x, 0, z);
                spawnPoint.y = terrain.SampleHeight(spawnPoint);

                float searchRadius = 100f;
                NavMeshHit hit;
                bool isOnNavMesh = NavMesh.SamplePosition(spawnPoint, out hit, searchRadius, NavMesh.AllAreas);

                if (!isOnNavMesh)
                {
                    Debug.Log($"Spawn point {spawnPoint} is NOT on the NavMesh.");
                    continue;
                }

                // Check if the path to the tower is valid
                NavMeshPath navMeshPath = new NavMeshPath();
                NavMesh.CalculatePath(hit.position, tower.position, NavMesh.AllAreas, navMeshPath);
                if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    validPoint = Vector3.Distance(hit.position, tower.position) >= minimumDistanceFromTower;
                }
                else
                {
                    Debug.LogWarning($"Invalid path from spawn point {spawnPoint}. Retrying...");
                }

                if (!validPoint)
                {
                    Debug.Log($"Attempt {attempts}: Spawn point {spawnPoint} was invalid.");
                }
            }

            if (validPoint)
            {
                points[i] = spawnPoint;
                Debug.Log($"Spawn point {spawnPoint} is valid and will be used.");
            }
            else
            {
                Debug.LogWarning($"Failed to find a valid spawn point after {maxAttempts} attempts.");
            }
        }

        return points;
    }


    public List<Vector3> GeneratePath(Vector3 spawnPoint)
    {
        List<Vector3> path = new List<Vector3>();

        if (tower != null)
        {
            NavMeshPath navMeshPath = new NavMeshPath();
            NavMesh.CalculatePath(spawnPoint, tower.position, NavMesh.AllAreas, navMeshPath);

            if (navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                path.AddRange(navMeshPath.corners);
                Debug.Log($"Generated path: {string.Join(" -> ", path.Select(p => p.ToString()))}");
            }
            else if (navMeshPath.status == NavMeshPathStatus.PathPartial)
            {
                Debug.LogWarning($"Path generated partially from {spawnPoint} to {tower.position}. Trying to adjust spawn point.");
                // Handle the partial path scenario (e.g., try another nearby position)
                path.Clear(); // Optionally, skip visualizing this path
            }
            else
            {
                Debug.LogError($"Failed to generate path. Status: {navMeshPath.status}, Start: {spawnPoint}, End: {tower.position}");
            }
        }
        else
        {
            Debug.LogError("Tower is not assigned. Cannot generate path.");
        }

        return path;
    }

    private void VisualizePath(List<Vector3> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.red, 10f);
        }
    }

    private bool IsPointBlockedByDefender(Vector3 point)
    {
        foreach (Vector3 defenderPosition in defenderPositions)
        {
            if (Vector3.Distance(point, defenderPosition) < defenderPlacementRadius)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsPointOnEnemyPath(Vector3 point)
    {
        foreach (Vector3 spawn in spawnPoints)
        {
            List<Vector3> path = GeneratePath(spawn);
            foreach (Vector3 pathPoint in path)
            {
                if (Vector3.Distance(point, pathPoint) < defenderPlacementRadius)
                {
                    return true;  // Point is too close to an enemy path
                }
            }
        }
        return false;  // Point is not on an enemy path
    }

    private bool CanPlaceDefender(Vector3 point)
    {
        return !IsPointBlockedByDefender(point) && !IsPointOnEnemyPath(point);
    }
}
