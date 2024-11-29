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
    private Terrain terrain; // Reference to the generated terrain
    private bool terrainReady = false; // Flag to check if terrain is ready

    void Start()
    {
        // Subscribe to terrain generation completion if TerrainGenerator exists
        TerrainGenerator generator = FindObjectOfType<TerrainGenerator>();
        if (generator != null)
        {
            generator.OnTerrainGenerated += HandleTerrainReady;
        }
        else
        {
            Debug.LogWarning("TerrainGenerator not found. Foliage will be placed immediately.");
            InitializeFoliage();
        }
    }

    private void HandleTerrainReady()
    {
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
        GenerateFoliage();
    }

    public void GenerateFoliage()
    {
        if (!terrainReady)
        {
            Debug.LogWarning("Terrain is not ready. Foliage generation skipped.");
            return;
        }

        for (int i = 0; i < maxFoliage; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            if (randomPosition != Vector3.zero) // Ensure the position is valid
            {
                GameObject prefab = foliagePrefabs[Random.Range(0, foliagePrefabs.Count)];
                GameObject foliage = Instantiate(prefab, randomPosition, Quaternion.identity);
                ApplyRandomTransform(foliage);
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        // Generate random X and Z positions within terrain bounds
        float randomX = Random.Range(0, terrainWidth);
        float randomZ = Random.Range(0, terrainLength);

        // Get terrain height at the random X, Z position
        float height = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

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

        // Ensure no collider is added
        Collider collider = foliage.GetComponent<Collider>();
        if (collider != null) Destroy(collider);
    }
}
