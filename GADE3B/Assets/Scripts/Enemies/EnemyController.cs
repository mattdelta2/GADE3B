using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }*/




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
    }
}



