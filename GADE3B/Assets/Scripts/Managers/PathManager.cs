using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathManager : MonoBehaviour
{/*
    public Terrain terrain;
    public Transform tower;  // The main tower's position
    public int numberOfSpawnPoints = 5;
    public float edgeMargin = 10f;  // Margin from the edge of the terrain

    private Vector3[] spawnPoints;

    void Start()
    {
        spawnPoints = GenerateSpawnPoints();
        // Generate path from each spawn point to the tower
        foreach (Vector3 spawn in spawnPoints)
        {
            List<Vector3> path = GeneratePath(spawn);
            // Optional: Visualize the path
            VisualizePath(path);
        }
    }

    Vector3[] GenerateSpawnPoints()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;

        Vector3[] points = new Vector3[numberOfSpawnPoints];
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            // Choose a random edge location
            bool isEdgeX = Random.value > 0.5f;
            bool isEdgeZ = Random.value > 0.5f;

            float x = isEdgeX ? (Random.value > 0.5f ? 0 : terrainWidth) : Random.Range(edgeMargin, terrainWidth - edgeMargin);
            float z = isEdgeZ ? (Random.value > 0.5f ? 0 : terrainHeight) : Random.Range(edgeMargin, terrainHeight - edgeMargin);

            points[i] = new Vector3(x, 0, z);
        }

        return points;
    }

    public List<Vector3> GeneratePath(Vector3 startPoint)
    {
        // For demonstration purposes, we use a simple straight path.
        // Replace this with your pathfinding algorithm.
        List<Vector3> path = new List<Vector3> { startPoint, tower.position };
        return path;
    }

    void VisualizePath(List<Vector3> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.red, 10f);
        }
    }
    


   
    public Terrain terrain;
    private Transform tower;  // The main tower's position
    public int numberOfSpawnPoints = 5;
    public float edgeMargin = 10f;  // Margin from the edge of the terrain

    private Vector3[] spawnPoints;

    void Start()
    {
        spawnPoints = GenerateSpawnPoints();
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
            // Optional: Visualize the path
            VisualizePath(path);
        }
    }

    public Vector3[] GenerateSpawnPoints()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;

        Vector3[] points = new Vector3[numberOfSpawnPoints];
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            bool isEdgeX = Random.value > 0.5f;
            bool isEdgeZ = Random.value > 0.5f;

            float x = isEdgeX ? (Random.value > 0.5f ? edgeMargin : terrainWidth - edgeMargin) : Random.Range(edgeMargin, terrainWidth - edgeMargin);
            float z = isEdgeZ ? (Random.value > 0.5f ? edgeMargin : terrainHeight - edgeMargin) : Random.Range(edgeMargin, terrainHeight - edgeMargin);

            points[i] = new Vector3(x, 0, z);
        }

        return points;
    }

    public List<Vector3> GeneratePath(Vector3 spawnPoint)
    {
        if (tower == null)
        {
            Debug.LogError("Tower is not assigned. Cannot generate path.");
            return new List<Vector3>(); // Return an empty list if tower is not assigned
        }

        List<Vector3> path = new List<Vector3>();
        // Sample pathfinding logic, you may need to replace this with a more sophisticated approach.
        path.Add(spawnPoint);
        path.Add(tower.position);

        Debug.Log("Path generated:");
        foreach (Vector3 point in path)
        {
            Debug.Log(point);
        }

        // Draw the path for debugging purposes
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.red, 10f);
        }

        return path; // Return the generated path
    }

    void VisualizePath(List<Vector3> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.red, 30f);
        }
    }



    public Terrain terrain;
    private Transform tower;  // The main tower's position
    public int numberOfSpawnPoints = 5;
    public float edgeMargin = 10f;  // Margin from the edge of the terrain

    private Vector3[] spawnPoints;

    void Start()
    {
        terrain = Terrain.activeTerrain;  // Get the active terrain in the scene
        if (terrain == null)
        {
            Debug.LogError("Terrain not found in the scene.");
        }

        spawnPoints = GenerateSpawnPoints();
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
            // Optional: Visualize the path
            VisualizePath(path);
        }
    }

    public Vector3[] GenerateSpawnPoints()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;

        Vector3[] points = new Vector3[numberOfSpawnPoints];
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            bool isEdgeX = Random.value > 0.5f;
            bool isEdgeZ = Random.value > 0.5f;

            float x = isEdgeX ? (Random.value > 0.5f ? edgeMargin : terrainWidth - edgeMargin) : Random.Range(edgeMargin, terrainWidth - edgeMargin);
            float z = isEdgeZ ? (Random.value > 0.5f ? edgeMargin : terrainHeight - edgeMargin) : Random.Range(edgeMargin, terrainHeight - edgeMargin);

            points[i] = new Vector3(x, 0, z);
        }

        return points;
    }

    public List<Vector3> GeneratePath(Vector3 spawnPoint)
    {
        if (tower == null)
        {
            Debug.LogError("Tower is not assigned. Cannot generate path.");
            return new List<Vector3>(); // Return an empty list if tower is not assigned
        }

        List<Vector3> path = new List<Vector3>();
        // Sample pathfinding logic, you may need to replace this with a more sophisticated approach.
        path.Add(spawnPoint);
        path.Add(tower.position);

        Debug.Log("Path generated:");
        foreach (Vector3 point in path)
        {
            Debug.Log(point);
        }

        // Draw the path for debugging purposes
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.red, 10f);
        }

        return path; // Return the generated path
    }

    void VisualizePath(List<Vector3> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.red, 30f);
        }
    }



   
    public Terrain terrain;
    private Transform tower;  // The main tower's position
    public int numberOfSpawnPoints = 5;
    public float edgeMargin = 10f;  // Margin from the edge of the terrain
    public float minimumDistanceFromTower = 10f;  // Minimum distance from the tower to place spawn points

    private Vector3[] spawnPoints;

    void Start()
    {
        if (terrain == null)
        {
            terrain = Terrain.activeTerrain;  // Get the active terrain in the scene
            if (terrain == null)
            {
                Debug.LogError("Terrain not found in the scene.");
                return;
            }
        }

        spawnPoints = GenerateSpawnPoints();
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
            // Optional: Visualize the path
            VisualizePath(path);
        }
    }

    public Vector3[] GenerateSpawnPoints()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;

        Vector3[] points = new Vector3[numberOfSpawnPoints];
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            Vector3 spawnPoint = Vector3.zero;  // Initialize the variable

            bool validPoint = false;
            while (!validPoint)
            {
                // Choose a random edge location
                bool isEdgeX = Random.value > 0.5f;
                bool isEdgeZ = Random.value > 0.5f;

                float x = isEdgeX ? (Random.value > 0.5f ? edgeMargin : terrainWidth - edgeMargin) : Random.Range(edgeMargin, terrainWidth - edgeMargin);
                float z = isEdgeZ ? (Random.value > 0.5f ? edgeMargin : terrainHeight - edgeMargin) : Random.Range(edgeMargin, terrainHeight - edgeMargin);

                spawnPoint = new Vector3(x, 0, z);
                spawnPoint.y = terrain.SampleHeight(spawnPoint);

                // Check if the point is at least `minimumDistanceFromTower` away from the tower
                if (tower != null)
                {
                    validPoint = Vector3.Distance(spawnPoint, tower.position) >= minimumDistanceFromTower;
                }
                else
                {
                    validPoint = true;  // No tower to check distance from
                }
            }

            points[i] = spawnPoint;
        }

        return points;
    }

    public List<Vector3> GeneratePath(Vector3 startPoint)
    {
        if (tower == null)
        {
            Debug.LogError("Tower is not assigned. Cannot generate path.");
            return new List<Vector3>(); // Return an empty list if tower is not assigned
        }

        List<Vector3> path = new List<Vector3> { startPoint, tower.position };

        Debug.Log("Path generated:");
        foreach (Vector3 point in path)
        {
            Debug.Log(point);
        }

        // Draw the path for debugging purposes
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.red, 10f);
        }

        return path;
    }

    void VisualizePath(List<Vector3> path)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1], Color.red, 30f);
        }
    } 





    
    public Terrain terrain;
    public int numberOfSpawnPoints = 5;
    public float edgeMargin = 10f;  // Margin from the edge of the terrain

    private Transform tower;  // Reference to the tower's position

    private void Start()
    {
        terrain = Terrain.activeTerrain;  // Automatically find the terrain if not assigned
        if (terrain == null)
        {
            Debug.LogError("Terrain not found in the scene.");
        }
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

        Vector3[] spawnPoints = GenerateSpawnPoints();
        foreach (Vector3 spawn in spawnPoints)
        {
            List<Vector3> path = GeneratePath(spawn);
            VisualizePath(path);  // Optional visualization for debugging
        }
    }

    public Vector3[] GenerateSpawnPoints()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;

        Vector3[] points = new Vector3[numberOfSpawnPoints];
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            bool isEdgeX = UnityEngine.Random.value > 0.5f;
            bool isEdgeZ = UnityEngine.Random.value > 0.5f;

            float x = isEdgeX ? (UnityEngine.Random.value > 0.5f ? edgeMargin : terrainWidth - edgeMargin) : UnityEngine.Random.Range(edgeMargin, terrainWidth - edgeMargin);
            float z = isEdgeZ ? (UnityEngine.Random.value > 0.5f ? edgeMargin : terrainHeight - edgeMargin) : UnityEngine.Random.Range(edgeMargin, terrainHeight - edgeMargin);

            Debug.Log($"Generated Spawn Point: {points[i]}");
            points[i] = new Vector3(x, 0, z);
            
        }

        return points;
    }

    public List<Vector3> GeneratePath(Vector3 spawnPoint)
    {
        List<Vector3> path = new List<Vector3>();

        if (tower != null)
        {
            path.Add(spawnPoint);
            path.Add(tower.position);
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
    }*/

    public Terrain terrain;
    public int numberOfSpawnPoints = 5;
    public float edgeMargin = 10f;  // Margin from the edge of the terrain

    private Transform tower;  // Reference to the tower's position

    private void Start()
    {
        terrain = Terrain.activeTerrain;  // Automatically find the terrain if not assigned
        if (terrain == null)
        {
            Debug.LogError("Terrain not found in the scene.");
        }
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

        Vector3[] spawnPoints = GenerateSpawnPoints();
        foreach (Vector3 spawn in spawnPoints)
        {
            List<Vector3> path = GeneratePath(spawn);
            VisualizePath(path);  // Optional visualization for debugging
        }
    }

    public Vector3[] GenerateSpawnPoints()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;

        Vector3[] points = new Vector3[numberOfSpawnPoints];
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            bool isEdgeX = UnityEngine.Random.value > 0.5f;
            bool isEdgeZ = UnityEngine.Random.value > 0.5f;

            float x = isEdgeX ? (UnityEngine.Random.value > 0.5f ? edgeMargin : terrainWidth - edgeMargin) : UnityEngine.Random.Range(edgeMargin, terrainWidth - edgeMargin);
            float z = isEdgeZ ? (UnityEngine.Random.value > 0.5f ? edgeMargin : terrainHeight - edgeMargin) : UnityEngine.Random.Range(edgeMargin, terrainHeight - edgeMargin);

            points[i] = new Vector3(x, 0, z);
        }

        return points;
    }

    public List<Vector3> GeneratePath(Vector3 spawnPoint)
    {
        List<Vector3> path = new List<Vector3>();

        if (tower != null)
        {
            path.Add(spawnPoint);
            path.Add(tower.position);

            // Debug output
            Debug.Log($"Generated path: {string.Join(" -> ", path.Select(p => p.ToString()))}");
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


}
