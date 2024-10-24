using UnityEngine;
using UnityEngine.AI;  // Import NavMesh functionality

public class TerrainGenerator : MonoBehaviour
{/*
    public Terrain terrain;  // Public field to assign the Terrain object
    public NavMeshSurface navMeshSurface;  // Reference to the NavMeshSurface component

    // Terrain settings
    public int width = 256;
    public int depth = 256;
    public int height;
    public float scale;  // Scale of the noise

    // Offsets for randomizing terrain
    public float offsetX = 100f;
    public float offsetZ = 100f;

    private bool navMeshReady = false;  // Flag to indicate if the NavMesh is baked and ready

    void Start()
    {
        // Check if the terrain has been assigned; if not, get it from the GameObject
        if (terrain == null)
        {
            terrain = GetComponent<Terrain>();
        }

        // Randomize terrain height and scale
        height = Random.Range(4, 15);
        scale = Random.Range(6, 20);

        // Randomize offsets for unique terrain each time
        offsetX = Random.Range(0f, 9999f);
        offsetZ = Random.Range(0f, 9999f);

        // Generate and apply the terrain data
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        // Check if NavMeshSurface is assigned and bake the NavMesh
        if (navMeshSurface != null)
        {
            BakeNavMesh();  // Bake NavMesh after terrain is generated
        }
        else
        {
            Debug.LogError("NavMeshSurface component is missing.");
        }
    }

    // Generate terrain data based on the heightmap
    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        // Set the resolution of the heightmap
        terrainData.heightmapResolution = width + 1;

        // Set the size of the terrain
        terrainData.size = new Vector3(width, height, depth);

        // Generate and smooth the heightmap, then apply it to the terrain
        terrainData.SetHeights(0, 0, SmoothHeights(GenerateHeights()));

        return terrainData;
    }

    // Generate a heightmap using Perlin noise
    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, depth];

        // Loop through each point on the heightmap
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                // Calculate the height at this point
                heights[x, z] = CalculateHeight(x, z);
            }
        }

        return heights;
    }

    // Calculate the height at a specific point using Perlin noise
    float CalculateHeight(int x, int z)
    {
        // Convert x and z coordinates to a range that Perlin noise can use
        float xCoord = (float)x / width * scale + offsetX;
        float zCoord = (float)z / depth * scale + offsetZ;

        // Return the noise value at this point, which determines the height
        return Mathf.PerlinNoise(xCoord, zCoord);
    }

    // Smooth the heightmap by averaging each point with its neighbors
    float[,] SmoothHeights(float[,] heights)
    {
        // Loop through each point on the heightmap, skipping the edges
        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < depth - 1; z++)
            {
                // Calculate the average height of the current point and its neighbors
                float avgHeight = (heights[x, z] +
                                   heights[x - 1, z] +
                                   heights[x + 1, z] +
                                   heights[x, z - 1] +
                                   heights[x, z + 1]) / 5f;

                // Set the current point to the averaged height
                heights[x, z] = avgHeight;
            }
        }

        return heights;
    }

    // Method to bake the NavMesh after terrain generation
    private void BakeNavMesh()
    {
        Debug.Log("Baking NavMesh...");
        navMeshSurface.BuildNavMesh();  // Build the NavMesh
        Debug.Log("NavMesh baked successfully.");
        navMeshReady = true;  // Set flag to indicate that the NavMesh is ready
    }

    // Public method to check if the NavMesh is ready
    public bool IsNavMeshReady()
    {
        return navMeshReady;
    }
*/
    public Terrain terrain;  // Public field to assign the Terrain object

    // Terrain settings
    public int width = 256;
    public int depth = 256;
    public int height;
    public float scale;  // Scale of the noise

    // Offsets for randomizing terrain
    public float offsetX = 100f;
    public float offsetZ = 100f;

    private bool navMeshReady = false;  // Flag to indicate if the NavMesh is baked and ready
    private NavMeshSurface navMeshSurface;  // Private reference to the NavMeshSurface component

    void Start()
    {
        // Check if the terrain has been assigned; if not, get it from the GameObject
        if (terrain == null)
        {
            terrain = GetComponentInChildren<Terrain>();
        }

        // Randomize terrain height and scale
        height = Random.Range(10, 20);
        scale = Random.Range(10, 20);

        // Randomize offsets for unique terrain each time
        offsetX = Random.Range(0f, 9999f);
        offsetZ = Random.Range(0f, 9999f);

        // Generate and apply the terrain data
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        // Create a NavMeshSurface component during runtime
        AssignOrCreateNavMeshSurface();

        // Check if NavMeshSurface is found and bake the NavMesh
        if (navMeshSurface != null)
        {
            // Delay baking for 1 frame to ensure terrain is fully generated
            Invoke(nameof(BakeNavMesh), 0.5f);  // Adjust delay if needed
        }
        else
        {
            Debug.LogError("NavMeshSurface component not found in the scene.");
        }
    }

    private void AssignOrCreateNavMeshSurface()
    {
        // Always create a new NavMeshSurface component on the terrain
        navMeshSurface = terrain.gameObject.AddComponent<NavMeshSurface>();
        Debug.Log("Created a new NavMeshSurface.");
    }

    // Generate terrain data based on the heightmap
    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        // Set the resolution of the heightmap
        terrainData.heightmapResolution = width + 1;

        // Set the size of the terrain
        terrainData.size = new Vector3(width, height, depth);

        // Generate and smooth the heightmap, then apply it to the terrain
        terrainData.SetHeights(0, 0, SmoothHeights(GenerateHeights()));

        return terrainData;
    }

    // Generate a heightmap using Perlin noise
    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, depth];

        // Loop through each point on the heightmap
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                // Calculate the height at this point
                heights[x, z] = CalculateHeight(x, z);
            }
        }

        return heights;
    }

    // Calculate the height at a specific point using Perlin noise
    float CalculateHeight(int x, int z)
    {
        // Convert x and z coordinates to a range that Perlin noise can use
        float xCoord = (float)x / width * scale + offsetX;
        float zCoord = (float)z / depth * scale + offsetZ;

        // Return the noise value at this point, which determines the height
        return Mathf.PerlinNoise(xCoord, zCoord);
    }

    // Smooth the heightmap by averaging each point with its neighbors
    float[,] SmoothHeights(float[,] heights)
    {
        // Loop through each point on the heightmap, skipping the edges
        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < depth - 1; z++)
            {
                // Calculate the average height of the current point and its neighbors
                float avgHeight = (heights[x, z] +
                                   heights[x - 1, z] +
                                   heights[x + 1, z] +
                                   heights[x, z - 1] +
                                   heights[x, z + 1]) / 5f;

                // Set the current point to the averaged height
                heights[x, z] = avgHeight;
            }
        }

        return heights;
    }

    // Method to bake the NavMesh after terrain generation
    private void BakeNavMesh()
    {
        Debug.Log("Baking NavMesh...");
        navMeshSurface.BuildNavMesh();  // Build the NavMesh
        Debug.Log("NavMesh baked successfully.");
        navMeshReady = true;  // Set flag to indicate that the NavMesh is ready
    }

    // Public method to re-bake NavMesh if terrain changes at runtime
    public void ReBakeNavMesh()
    {
        if (navMeshSurface == null)
        {
            Debug.LogError("NavMeshSurface is not assigned, cannot rebake NavMesh.");
            return;
        }

        Debug.Log("Re-baking NavMesh...");
        navMeshSurface.BuildNavMesh();
        Debug.Log("NavMesh re-baked.");
    }

    // Public method to check if the NavMesh is ready
    public bool IsNavMeshReady()
    {
        return navMeshReady;
    }






    /*more random terrain generation

    public Terrain terrain;

    // Terrain settings
    public int width = 256;
    public int depth = 256;
    public int maxHeight = 40;  // Maximum height of the terrain
    public float baseScale = 50f;  // Base scale of the terrain noise
    public float ridgeScale = 30f;  // Scale for ridged noise
    public float turbulenceFactor = 20f;  // Factor for adding turbulence (warping the terrain)

    // Noise parameters
    public int octaves = 5;  // Number of noise layers (octaves)
    public float persistence = 0.5f;  // Determines how much each octave affects the terrain
    public float lacunarity = 2.0f;  // Determines the frequency of each octave

    private NavMeshSurface navMeshSurface;

    void Start()
    {
        if (terrain == null)
        {
            terrain = GetComponentInChildren<Terrain>();
        }

        // Generate and apply terrain data
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        AssignOrCreateNavMeshSurface();

        if (navMeshSurface != null)
        {
            Invoke(nameof(BakeNavMesh), 0.5f);
        }
        else
        {
            Debug.LogError("NavMeshSurface component not found.");
        }
    }

    private void AssignOrCreateNavMeshSurface()
    {
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            navMeshSurface = terrain.gameObject.AddComponent<NavMeshSurface>();
        }
    }

    // Generate terrain data based on multiple layers of noise and ridged noise
    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, maxHeight, depth);
        terrainData.SetHeights(0, 0, GenerateHeightmap());

        return terrainData;
    }

    // Generate heightmap using a combination of regular and ridged noise, with turbulence
    float[,] GenerateHeightmap()
    {
        float[,] heights = new float[width, depth];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                // Get the height value by combining regular Perlin noise and ridged noise
                heights[x, z] = Mathf.Clamp01(GenerateTurbulentHeight(x, z));
            }
        }

        return heights;
    }

    // Generate height using Perlin noise, ridged noise, and turbulence
    float GenerateTurbulentHeight(int x, int z)
    {
        float amplitude = 1f;
        float frequency = 1f;
        float heightValue = 0f;

        // Combine multiple layers of Perlin noise to create complex terrain features
        for (int i = 0; i < octaves; i++)
        {
            float xCoord = (float)x / width * baseScale * frequency;
            float zCoord = (float)z / depth * baseScale * frequency;

            // Apply Perlin noise for base terrain
            heightValue += Mathf.PerlinNoise(xCoord, zCoord) * amplitude;

            // Apply ridged noise for sharper, mountain-like features
            heightValue += RidgedNoise(xCoord * ridgeScale, zCoord * ridgeScale) * amplitude * 0.5f;

            // Add turbulence by warping coordinates
            xCoord += Turbulence(x, z) * turbulenceFactor;
            zCoord += Turbulence(z, x) * turbulenceFactor;

            // Update amplitude and frequency for the next octave
            amplitude *= persistence;
            frequency *= lacunarity;
        }

        return heightValue;
    }

    // Ridged noise creates sharp, mountain-like ridges
    float RidgedNoise(float x, float z)
    {
        float noise = Mathf.PerlinNoise(x, z);
        noise = 1.0f - Mathf.Abs(noise);  // Invert and clamp the noise for ridged effect
        return noise * noise;  // Square to emphasize the ridges
    }

    // Turbulence function warps the coordinates fed into the noise functions for random, natural terrain
    float Turbulence(int x, int z)
    {
        float turbulence = Mathf.PerlinNoise((float)x / width * ridgeScale, (float)z / depth * ridgeScale);
        return turbulence * 2f - 1f;  // Return a value in the range of [-1, 1] for warping
    }

    private void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
        Debug.Log("NavMesh baked successfully.");
    }

    public void ReBakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

    public bool IsNavMeshReady()
    {
        return navMeshSurface != null;
    }


    /**/
}


