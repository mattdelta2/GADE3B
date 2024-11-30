using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Terrain Settings")]
    public Terrain terrain;              // Assign the Terrain object
    public int width = 256;              // Terrain width
    public int depth = 256;              // Terrain depth
    public int height;                   // Terrain height, randomized
    public float scale;                  // Scale for Perlin noise

    [Header("Randomization Offsets")]
    public float offsetX;                // Random X offset
    public float offsetZ;                // Random Z offset

    [Header("NavMesh")]
    private NavMeshSurface navMeshSurface; // NavMeshSurface for navigation
    private bool navMeshReady = false;     // Flag to check NavMesh readiness

    public event Action OnTerrainGenerated; // Event to notify when terrain generation is complete

    void Start()
    {
        Debug.Log("Starting TerrainGenerator...");
        AssignTerrain();

        if (terrain == null)
        {
            Debug.LogError("No terrain assigned or found in the scene.");
            return;
        }

        RandomizeTerrainSettings();
        Debug.Log($"Randomized Terrain Settings: Height={height}, Scale={scale}, Offsets=({offsetX}, {offsetZ})");

        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        Debug.Log("Terrain data generated.");

        AssignOrCreateNavMeshSurface();

        if (navMeshSurface != null)
        {
            // Delay NavMesh baking slightly to ensure terrain is fully generated
            Invoke(nameof(BakeNavMesh), 0.5f);
        }
        else
        {
            Debug.LogError("NavMeshSurface component is missing.");
        }

        // Notify any listeners that terrain generation is complete
        OnTerrainGenerated?.Invoke();
        Debug.Log("OnTerrainGenerated event invoked.");
    }

    /// <summary>
    /// Assign the Terrain component if not already assigned.
    /// </summary>
    private void AssignTerrain()
    {
        if (terrain == null)
        {
            terrain = GetComponentInChildren<Terrain>();
            Debug.Log(terrain != null ? "Terrain assigned." : "No terrain found in children.");
        }
    }

    /// <summary>
    /// Randomize terrain properties such as height, scale, and offsets.
    /// </summary>
    private void RandomizeTerrainSettings()
    {
        height = Random.Range(10, 20);
        scale = Random.Range(10, 20);
        offsetX = Random.Range(0f, 9999f);
        offsetZ = Random.Range(0f, 9999f);
    }

    /// <summary>
    /// Generate terrain data based on Perlin noise.
    /// </summary>
    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, height, depth);
        terrainData.SetHeights(0, 0, SmoothHeights(GenerateHeights()));
        return terrainData;
    }

    /// <summary>
    /// Generate a heightmap using Perlin noise.
    /// </summary>
    private float[,] GenerateHeights()
    {
        float[,] heights = new float[width, depth];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                heights[x, z] = CalculateHeight(x, z);
            }
        }
        return heights;
    }

    /// <summary>
    /// Calculate Perlin noise value for a given point.
    /// </summary>
    private float CalculateHeight(int x, int z)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float zCoord = (float)z / depth * scale + offsetZ;
        return Mathf.PerlinNoise(xCoord, zCoord);
    }

    /// <summary>
    /// Smooth the heightmap to reduce sharp terrain features.
    /// </summary>
    private float[,] SmoothHeights(float[,] heights)
    {
        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < depth - 1; z++)
            {
                heights[x, z] = (
                    heights[x, z] +
                    heights[x - 1, z] +
                    heights[x + 1, z] +
                    heights[x, z - 1] +
                    heights[x, z + 1]
                ) / 5f;
            }
        }
        return heights;
    }

    /// <summary>
    /// Assign or create the NavMeshSurface component.
    /// </summary>
    private void AssignOrCreateNavMeshSurface()
    {
        navMeshSurface = terrain.gameObject.GetComponent<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            navMeshSurface = terrain.gameObject.AddComponent<NavMeshSurface>();
            Debug.Log("Created a new NavMeshSurface.");
        }
        else
        {
            Debug.Log("NavMeshSurface already exists.");
        }
    }

    /// <summary>
    /// Bake the NavMesh for pathfinding.
    /// </summary>
    private void BakeNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
            navMeshReady = true;
            Debug.Log("NavMesh baked successfully.");
        }
        else
        {
            Debug.LogError("NavMeshSurface is null. Cannot bake NavMesh.");
        }
    }

    /// <summary>
    /// Re-bake the NavMesh during runtime if terrain changes.
    /// </summary>
    public void ReBakeNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
            Debug.Log("NavMesh re-baked successfully.");
        }
        else
        {
            Debug.LogError("NavMeshSurface is not assigned.");
        }
    }

    /// <summary>
    /// Check if the NavMesh is ready.
    /// </summary>
    public bool IsNavMeshReady()
    {
        return navMeshReady;
    }
}
