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
        height = Random.Range(4, 15);
        scale = Random.Range(6, 20);

        // Randomize offsets for unique terrain each time
        offsetX = Random.Range(0f, 9999f);
        offsetZ = Random.Range(0f, 9999f);

        // Generate and apply the terrain data
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        // Find or create the NavMeshSurface component in the scene during runtime
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
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            Debug.LogWarning("NavMeshSurface not found. Creating a new one.");
            navMeshSurface = terrain.gameObject.AddComponent<NavMeshSurface>();
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

    // Public method to re-bake NavMesh if terrain changes at runtime
    public void ReBakeNavMesh()
    {
        Debug.Log("Re-baking NavMesh...");
        navMeshSurface.BuildNavMesh();
        Debug.Log("NavMesh re-baked.");
    }

    // Public method to check if the NavMesh is ready
    public bool IsNavMeshReady()
    {
        return navMeshReady;
    }

    */




    /*more random terrain generation*/
    
    public Terrain terrain;  // Public field to assign the Terrain object

    // Terrain settings
    public int width = 256;
    public int depth = 256;
    public int baseHeight = 10;  // Base height for the terrain
    public float scale = 20f;  // Scale of the noise
    public float frequency = 2f;  // Frequency of the Perlin noise for roughness
    public int octaves = 4;  // Number of noise layers to add complexity
    public float persistence = 0.5f;  // Controls amplitude of each octave
    public float lacunarity = 2f;  // Controls frequency increase per octave

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

        // Randomize terrain height and noise scale
        baseHeight = Random.Range(8, 15);
        scale = Random.Range(10f, 30f);
        frequency = Random.Range(1.5f, 3.5f);

        // Randomize offsets for unique terrain each time
        offsetX = Random.Range(0f, 9999f);
        offsetZ = Random.Range(0f, 9999f);

        // Generate and apply the terrain data
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        // Find or create the NavMeshSurface component in the scene during runtime
        AssignOrCreateNavMeshSurface();

        // Check if NavMeshSurface is found and bake the NavMesh
        if (navMeshSurface != null)
        {
            Invoke(nameof(BakeNavMesh), 0.5f);  // Delay baking for 1 frame to ensure terrain is fully generated
        }
        else
        {
            Debug.LogError("NavMeshSurface component not found in the scene.");
        }
    }

    private void AssignOrCreateNavMeshSurface()
    {
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            Debug.LogWarning("NavMeshSurface not found. Creating a new one.");
            navMeshSurface = terrain.gameObject.AddComponent<NavMeshSurface>();
        }
    }

    // Generate terrain data based on the heightmap
    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        // Set the resolution of the heightmap
        terrainData.heightmapResolution = width + 1;

        // Set the size of the terrain
        terrainData.size = new Vector3(width, baseHeight, depth);

        // Generate and smooth the heightmap, then apply it to the terrain
        terrainData.SetHeights(0, 0, SmoothHeights(GenerateHeights()));

        return terrainData;
    }

    // Generate a heightmap using multiple Perlin noise layers (octaves)
    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, depth];

        // Loop through each point on the heightmap
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                heights[x, z] = GeneratePerlinNoise(x, z);
            }
        }

        return heights;
    }

    // Generate terrain using multiple layers of Perlin noise (octaves)
    float GeneratePerlinNoise(int x, int z)
    {
        float height = 0f;
        float amplitude = 1f;
        float frequency = this.frequency;
        float maxPossibleHeight = 0f;

        // Add multiple noise layers (octaves) for more varied terrain
        for (int i = 0; i < octaves; i++)
        {
            float xCoord = (float)x / width * frequency + offsetX;
            float zCoord = (float)z / depth * frequency + offsetZ;

            // Add Perlin noise with decreasing amplitude and increasing frequency
            height += Mathf.PerlinNoise(xCoord, zCoord) * amplitude;

            maxPossibleHeight += amplitude;

            amplitude *= persistence;  // Reduce amplitude for next octave
            frequency *= lacunarity;   // Increase frequency for next octave
        }

        return height / maxPossibleHeight;  // Normalize the height value
    }

    // Smooth the heightmap by averaging each point with its neighbors
    float[,] SmoothHeights(float[,] heights)
    {
        // Loop through each point on the heightmap, skipping the edges
        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < depth - 1; z++)
            {
                float avgHeight = (heights[x, z] +
                                   heights[x - 1, z] +
                                   heights[x + 1, z] +
                                   heights[x, z - 1] +
                                   heights[x, z + 1]) / 5f;

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
        Debug.Log("Re-baking NavMesh...");
        navMeshSurface.BuildNavMesh();
        Debug.Log("NavMesh re-baked.");
    }

    // Public method to check if the NavMesh is ready
    public bool IsNavMeshReady()
    {
        return navMeshReady;
    }


    /**/
}


