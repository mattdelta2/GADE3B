using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class FoliageManager : MonoBehaviour
{
    [Header("Foliage Settings")]
    public List<GameObject> foliagePrefabs; // List of foliage prefabs
    public int maxFoliage = 100; // Maximum number of foliage instances
    public float minScale = 0.5f; // Minimum scale for foliage
    public float maxScale = 1.5f; // Maximum scale for foliage

    [Header("Dependencies")]
    public LayerMask terrainLayer; // Terrain layer for placement
    public Terrain terrain; // Reference to the generated terrain
    private bool terrainReady = false; // Flag to check if terrain is ready

    public void Start()
    {
        // Debug start of foliage generation
        Debug.Log("FoliageManager started.");

        // Subscribe to terrain generation completion if TerrainGenerator exists
        TerrainGenerator generator = FindObjectOfType<TerrainGenerator>();
        if (generator != null)
        {
            generator.OnTerrainGenerated += HandleTerrainReady;
            Debug.Log("Subscribed to TerrainGenerator's OnTerrainGenerated event.");
        }
        else
        {
            Debug.LogWarning("TerrainGenerator not found. Foliage will be placed immediately.");
            InitializeFoliage();
        }
    }

    private void HandleTerrainReady()
    {
        Debug.Log("Terrain generation completed. Proceeding with foliage placement.");
        terrain = FindObjectOfType<Terrain>(); // Get reference to the generated terrain
        if (terrain != null)
        {
            InitializeFoliage();
        }
        else
        {
            Debug.LogError("Terrain not found. Foliage generation aborted.");
        }
    }

    private void InitializeFoliage()
    {
        terrainReady = true;
        Debug.Log("Initializing foliage generation...");
        GenerateFoliage();
    }

    public void GenerateFoliage()
    {
        if (!terrainReady)
        {
            Debug.LogWarning("Terrain is not ready. Foliage generation skipped.");
            return;
        }

        if (foliagePrefabs == null || foliagePrefabs.Count == 0)
        {
            Debug.LogError("Foliage prefabs list is empty or not assigned.");
            return;
        }

        Debug.Log($"Starting foliage generation with {maxFoliage} instances.");
        for (int i = 0; i < maxFoliage; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            if (randomPosition != Vector3.zero) // Ensure the position is valid
            {
                GameObject prefab = foliagePrefabs[Random.Range(0, foliagePrefabs.Count)];
                if (prefab != null)
                {
                    GameObject foliage = Instantiate(prefab, randomPosition, Quaternion.identity);
                    ApplyRandomTransform(foliage);
                    Debug.Log($"Foliage placed: {prefab.name} at position {randomPosition}");
                }
                else
                {
                    Debug.LogError("Randomly selected foliage prefab is null.");
                }
            }
            else
            {
                Debug.LogWarning("Invalid position generated for foliage.");
            }
        }
        Debug.Log("Foliage generation completed.");
    }

    private Vector3 GetRandomPosition()
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain reference is null. Cannot generate random positions.");
            return Vector3.zero;
        }

        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        // Generate random X and Z positions within terrain bounds
        float randomX = Random.Range(0, terrainWidth);
        float randomZ = Random.Range(0, terrainLength);

        // Get terrain height at the random X, Z position
        float height = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

        // Debugging position and height
        Debug.Log($"Generated random position: X={randomX}, Z={randomZ}, Height={height}");

        // Ensure the position is above ground level
        if (height > 0.1f)
        {
            return new Vector3(randomX, height, randomZ);
        }
        return Vector3.zero; // Invalid position
    }

    private void ApplyRandomTransform(GameObject foliage)
    {
        // Apply random scaling
        float randomScale = Random.Range(minScale, maxScale);
        foliage.transform.localScale = Vector3.one * randomScale;

        // Apply random rotation
        foliage.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        Debug.Log($"Applied random transform: Scale={randomScale}");

        // Ensure no collider is added
        Collider collider = foliage.GetComponent<Collider>();
        if (collider != null)
        {
            Destroy(collider);
            Debug.Log("Removed collider from foliage.");
        }
    }
}
