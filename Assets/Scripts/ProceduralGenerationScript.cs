using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProceduralGenerationScript : MonoBehaviour
{
    private enum BlockType
    {
        WATER,
        SAND,
        GRASS,
        STONE,
        SNOW
    }

    public GameObject[] blockPrefabs;

    public GameObject[] surfaceBlockPrefabs;

    public GameObject[] treePrefabs;

    public GameObject[] palmPrefabs;

    public int HEIGHT = 64;
    public int WIDTH = 64;
    public int DEPTH = 16;

    private float[,] evalution;
    private static readonly float[] noise = { 1.00f, 0.50f, 0.25f, 0.13f, 0.06f, 0.03f };

    // Spawn point for player that avoids water
    private Vector3 playerSpawnPoint;

    void Start()
    {
        evalution = new float[WIDTH, HEIGHT];
        Generate();
        LocatePlayerSpawnPoint();
        Draw();
        GrowPoissonForest();
    }

    void Draw()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                int height = (int)(evalution[x, y] * DEPTH);
                BlockType type = Biome(evalution[x, y]);
                if (type == BlockType.WATER) {
                    StartCoroutine(WaterColumn(x, y, type));
                }
                else {
                    if (type == BlockType.SAND && height < 2) height = 2;
                    StartCoroutine(Column(x, y, height, type));
                }
            }
        }

    }

    IEnumerator WaterColumn(int x, int y, BlockType type)
    {
        Instantiate(blockPrefabs[(int)type], new Vector3(x, 0, y), Quaternion.identity);
        Instantiate(surfaceBlockPrefabs[(int)type], new Vector3(x, 2, y), Quaternion.identity);
        yield return null;
    }

    IEnumerator Column(int x, int y, int height, BlockType type)
    {
        GameObject block = blockPrefabs[(int)type];
        for (int z = 0; z < height - 1; z++)
        {
            Instantiate(block, new Vector3(x, z, y), Quaternion.identity);
            yield return null;
        }
        Instantiate(surfaceBlockPrefabs[(int)type], new Vector3(x, height - 1, y), Quaternion.identity);
    }

    void GrowPoissonForest()
    {
        PoissonDiscSampler sampler = new(WIDTH, HEIGHT, 16);
        foreach (Vector2 sample in sampler.Samples())
        {
            int height = (int)(evalution[(int)sample.x, (int)sample.y] * DEPTH);
            if (Biome(evalution[(int)sample.x, (int)sample.y]) == BlockType.GRASS)
            {
                GameObject tree = treePrefabs[UnityEngine.Random.Range(0, treePrefabs.Length)];
                Instantiate(tree, new Vector3(sample.x, height, sample.y), tree.transform.rotation);
            }
            else if (Biome(evalution[(int)sample.x, (int)sample.y]) == BlockType.SAND)
            {
                GameObject palm = palmPrefabs[UnityEngine.Random.Range(0, palmPrefabs.Length)];
                Instantiate(palm, new Vector3(sample.x, height, sample.y), palm.transform.rotation);
            }
        }
    }

    void Generate()
    {
        float SEED = UnityEngine.Random.Range(-0.5f, 0.5f);
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                float nx = x / (float)WIDTH + SEED;
                float ny = y / (float)HEIGHT + SEED;
                float e = noise[0] * Unity.Mathematics.noise.snoise(new Unity.Mathematics.float2(nx, ny))
                    + noise[1] * Unity.Mathematics.noise.snoise(new Unity.Mathematics.float2(2 * nx, 2 * ny))
                    + noise[2] * Unity.Mathematics.noise.snoise(new Unity.Mathematics.float2(4 * nx, 4 * ny))
                    + noise[3] * Unity.Mathematics.noise.snoise(new Unity.Mathematics.float2(8 * nx, 8 * ny))
                    + noise[4] * Unity.Mathematics.noise.snoise(new Unity.Mathematics.float2(16 * nx, 16 * ny))
                    + noise[5] * Unity.Mathematics.noise.snoise(new Unity.Mathematics.float2(32 * nx, 32 * ny));
                e /= noise.Sum();
                evalution[x, y] = e / 2f + 0.5f;
            }
        }
    }

    void LocatePlayerSpawnPoint()
    {
        // BFS to find a spawn point that is not water
        Vector3 center = new(WIDTH / 2, 0, HEIGHT / 2);
        Queue<Vector3> queue = new();
        queue.Enqueue(center);
        while (queue.Count > 0)
        {
            Vector3 current = queue.Dequeue();
            if (Biome(evalution[(int)current.x, (int)current.z]) != BlockType.WATER)
            {
                playerSpawnPoint = current;
                playerSpawnPoint.y = DEPTH + 1;
                return;
            }
            if (current.x > 0) queue.Enqueue(current + Vector3.left);
            if (current.x < WIDTH - 1) queue.Enqueue(current + Vector3.right);
            if (current.z > 0) queue.Enqueue(current + Vector3.back);
            if (current.z < HEIGHT - 1) queue.Enqueue(current + Vector3.forward);
        }
    }

    BlockType Biome(float e) {
        if (e < 0.2f) return BlockType.WATER;
        else if (e < 0.35f) return BlockType.SAND;
        else if (e < 0.6f) return BlockType.GRASS;
        else if (e < 0.8f) return BlockType.STONE;
        else return BlockType.SNOW;
    }
}