using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // Public field to assign the Terrain object from the Inspector
    public Terrain terrain;

    // Terrain settings
    public int width = 256;
    public int depth = 256;
    public int height;
    public float scale; // Scale of the noise

    // Offsets for randomizing terrain
    public float offsetX = 100f;
    public float offsetZ = 100f;

    void Start()
    {
        // Check if the terrain has been assigned; if not, get it from the GameObject
        if (terrain == null)
        {
            terrain = GetComponent<Terrain>();
        }

         height = Random.Range(4, 15);
         scale = Random.Range(6,20);

        // Randomize offsets for unique terrain each time
        offsetX = Random.Range(0f, 9999f);
        offsetZ = Random.Range(0f, 9999f);

        // Generate and apply the terrain data
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
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
}
