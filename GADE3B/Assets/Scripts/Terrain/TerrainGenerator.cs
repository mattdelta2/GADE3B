using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Terrain Settings")]
    public Terrain terrain;
    public int width = 256;
    public int depth = 256;
    public int height;
    public float scale;

    [Header("Randomization Offsets")]
    public float offsetX;
    public float offsetZ;

    [Header("NavMesh")]
    private NavMeshSurface navMeshSurface;
    private bool navMeshReady = false;

    public event Action OnTerrainGenerated;

    [Header("Foliage Settings")]
    public List<GameObject> foliagePrefabs;
    public int maxFoliage = 100;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;

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
            Invoke(nameof(BakeNavMesh), 0.5f);
        }
        else
        {
            Debug.LogError("NavMeshSurface component is missing.");
        }

        OnTerrainGenerated?.Invoke();
        Debug.Log("OnTerrainGenerated event invoked.");

        GenerateFoliage();
    }

    private void AssignTerrain()
    {
        if (terrain == null)
        {
            terrain = GetComponentInChildren<Terrain>();
            Debug.Log(terrain != null ? "Terrain assigned." : "No terrain found in children.");
        }
    }

    private void RandomizeTerrainSettings()
    {
        height = Random.Range(10, 20);
        scale = Random.Range(10, 20);
        offsetX = Random.Range(0f, 9999f);
        offsetZ = Random.Range(0f, 9999f);
    }

    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, height, depth);
        terrainData.SetHeights(0, 0, SmoothHeights(GenerateHeights()));
        return terrainData;
    }

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

    private float CalculateHeight(int x, int z)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float zCoord = (float)z / depth * scale + offsetZ;
        return Mathf.PerlinNoise(xCoord, zCoord);
    }

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

    public bool IsNavMeshReady()
    {
        return navMeshReady;
    }

    private void GenerateFoliage()
    {
        if (foliagePrefabs == null || foliagePrefabs.Count == 0)
        {
            Debug.LogError("Foliage prefabs list is empty or not assigned.");
            return;
        }

        Debug.Log($"Starting foliage generation with {maxFoliage} instances.");
        for (int i = 0; i < maxFoliage; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            if (randomPosition != Vector3.zero)
            {
                GameObject prefab = foliagePrefabs[Random.Range(0, foliagePrefabs.Count)];
                if (prefab != null)
                {
                    GameObject foliage = Instantiate(prefab, randomPosition, Quaternion.identity);
                    ApplyRandomTransform(foliage);
                    Debug.Log($"Foliage placed: {prefab.name} at position {randomPosition}");
                }
            }
        }
        Debug.Log("Foliage generation completed.");
    }

    private Vector3 GetRandomPosition()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        float randomX = Random.Range(0, terrainWidth);
        float randomZ = Random.Range(0, terrainLength);

        float height = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));
        if (height > 0.1f)
        {
            return new Vector3(randomX, height, randomZ);
        }
        return Vector3.zero;
    }

    private void ApplyRandomTransform(GameObject foliage)
    {
        float randomScale = Random.Range(minScale, maxScale);
        foliage.transform.localScale = Vector3.one * randomScale;
        foliage.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        Collider collider = foliage.GetComponent<Collider>();
        if (collider != null)
        {
            Destroy(collider);
        }
    }
}
