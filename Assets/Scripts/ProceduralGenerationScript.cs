using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sample;
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
    public GameObject bedrockPrefab;
    public GameObject goalPrefab;
    public int HEIGHT = 64;
    public int WIDTH = 64;
    public int DEPTH = 16;

    private float[,] evalution;
    private static readonly float[] noise = { 1.00f, 0.50f, 0.25f, 0.13f, 0.06f, 0.03f };

    private readonly HashSet<Vector2> blacklist = new();
    public int playerGoalMinimumDistance = 12;

    void Start()
    {
        evalution = new float[WIDTH, HEIGHT];
        Generate();
        Draw();
        GrowPoissonForest();
        LocatePlayerSpawnPoint();
        StartCoroutine(GenerateGoal());
    }

    void Draw()
    {
        // Inner blocks
        for (int x = 1; x < WIDTH - 1; x++)
        {
            for (int y = 1; y < HEIGHT - 1; y++)
            {
                int height = Height(evalution[x, y]);
                BlockType type = Biome(evalution[x, y]);
                if (type == BlockType.WATER) {
                    StartCoroutine(WaterColumn(x, y, type));
                    blacklist.Add(new Vector2(x, y));
                }
                else {
                    StartCoroutine(Column(x, y, height, type));
                }
            }
        }
        // Outer blocks
        for (int x = 0; x < WIDTH; x++)
        {
            int height = Height(evalution[x, 0]);
            BlockType type = Biome(evalution[x, 0]);
            StartCoroutine(BorderColumn(x, 0, height, type));
            height = Height(evalution[x, HEIGHT - 1]);
            type = Biome(evalution[x, HEIGHT - 1]);
            StartCoroutine(BorderColumn(x, HEIGHT - 1, height, type));
        }
        for (int y = 1; y < HEIGHT - 1; y++)
        {
            int height = Height(evalution[0, y]);
            BlockType type = Biome(evalution[0, y]);
            StartCoroutine(BorderColumn(0, y, height, type));
            height = Height(evalution[WIDTH - 1, y]);
            type = Biome(evalution[WIDTH - 1, y]);
            StartCoroutine(BorderColumn(WIDTH - 1, y, height, type));
        }
    }

    IEnumerator WaterColumn(int x, int y, BlockType type)
    {
        Instantiate(surfaceBlockPrefabs[(int)type], new Vector3(x, 2, y), Quaternion.identity);
        Instantiate(blockPrefabs[(int)type], new Vector3(x, 0, y), Quaternion.identity);
        yield return null;
    }

    IEnumerator Column(int x, int y, int height, BlockType type)
    {
        Instantiate(surfaceBlockPrefabs[(int)type], new Vector3(x, height - 1, y), Quaternion.identity);
        yield return null;
        GameObject block = blockPrefabs[(int)type];
        int d = MaxHeightDifference(x, y);
        for (int z = height - d; z < height - 1; z++)
        {
            Instantiate(block, new Vector3(x, z, y), Quaternion.identity);
            yield return null;
        }
    }

    IEnumerator BorderColumn(int x, int y, int height, BlockType type)
    {
        Instantiate(surfaceBlockPrefabs[(int)type], new Vector3(x, height - 1, y), Quaternion.identity);
        yield return null;
        Instantiate(bedrockPrefab, new Vector3(x, 0, y), Quaternion.identity);
        GameObject block = blockPrefabs[(int)type];
        for (int z = 1; z < height - 1; z++)
        {
            Instantiate(block, new Vector3(x, z, y), Quaternion.identity);
            yield return null;
        }
    }

    int MaxHeightDifference(int x, int y)
    {
        int dLeft = HeightDifference(evalution[x, y], evalution[x - 1, y]);
        int dRight =HeightDifference(evalution[x, y], evalution[x + 1, y]);
        int dBack = HeightDifference(evalution[x, y], evalution[x, y - 1]);
        int dForward = HeightDifference(evalution[x, y], evalution[x, y + 1]);
        return Math.Max(dLeft, Math.Max(dRight, Math.Max(dBack, dForward)));
    }

    int MinHeightDifference(int x, int y)
    {
        int dLeft = x > 0 ? HeightDifference(evalution[x, y], evalution[x - 1, y]) : int.MaxValue;
        int dRight = x < WIDTH - 1 ? HeightDifference(evalution[x, y], evalution[x + 1, y]) : int.MaxValue;
        int dBack = y > 0 ? HeightDifference(evalution[x, y], evalution[x, y - 1]) : int.MaxValue;
        int dForward = y < HEIGHT - 1 ? HeightDifference(evalution[x, y], evalution[x, y + 1]) : int.MaxValue;
        return Math.Min(dLeft, Math.Min(dRight, Math.Min(dBack, dForward)));
    }

    int HeightDifference(float here, float there)
    {
        if (Biome(there) == BlockType.WATER) {
            return Mathf.RoundToInt(here * DEPTH);
        }
        // Make sure the height difference is at least 1
        return Mathf.CeilToInt(Math.Abs(here - there) * DEPTH);
    }

    void GrowPoissonForest()
    {
        PoissonDiscSampler sampler = new(WIDTH, HEIGHT, 16);
        foreach (Vector2 sample in sampler.Samples())
        {
            int height = Height(evalution[(int)sample.x, (int)sample.y]);
            if (Biome(evalution[(int)sample.x, (int)sample.y]) == BlockType.GRASS)
            {
                GameObject tree = treePrefabs[UnityEngine.Random.Range(0, treePrefabs.Length)];
                Instantiate(tree, new Vector3(sample.x, height, sample.y), tree.transform.rotation);
                blacklist.Add(sample);
            }
            else if (Biome(evalution[(int)sample.x, (int)sample.y]) == BlockType.SAND)
            {
                GameObject palm = palmPrefabs[UnityEngine.Random.Range(0, palmPrefabs.Length)];
                Instantiate(palm, new Vector3(sample.x, height, sample.y), palm.transform.rotation);
                blacklist.Add(sample);
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
            if (!blacklist.Contains(new Vector2(current.x, current.z)))
            {
                Vector3 playerSpawnPoint = current;
                playerSpawnPoint.y = Height(evalution[(int)current.x, (int)current.z]);
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<GhostScript>().SetRespawnPosition(playerSpawnPoint);
                player.transform.position = playerSpawnPoint;
                blacklist.UnionWith(Enumerable.Range(-playerGoalMinimumDistance, playerGoalMinimumDistance * 2 + 1)
                    .SelectMany(i => Enumerable.Range(-playerGoalMinimumDistance, playerGoalMinimumDistance * 2 + 1)
                    .Select(j => new Vector2(current.x + i, current.z + j))));
                return;
            }
            if (current.x > 0) queue.Enqueue(current + Vector3.left);
            if (current.x < WIDTH - 1) queue.Enqueue(current + Vector3.right);
            if (current.z > 0) queue.Enqueue(current + Vector3.back);
            if (current.z < HEIGHT - 1) queue.Enqueue(current + Vector3.forward);
        }
    }

    IEnumerator GenerateGoal() {
        // Select a random point that is not water and is not in the blacklist
        Vector3 goal;
        do
        {
            Vector2 temp = new(UnityEngine.Random.Range(0, WIDTH), UnityEngine.Random.Range(0, HEIGHT));
            goal = new Vector3(temp.x, 0, temp.y);
            blacklist.Add(temp);
            yield return null;
        } while (blacklist.Contains(new Vector2(goal.x, goal.z)) && MinHeightDifference((int)goal.x, (int)goal.z) > 1);
        goal.y = Height(evalution[(int)goal.x, (int)goal.z]);
        Instantiate(goalPrefab, goal, Quaternion.identity);
    }

    BlockType Biome(float e) {
        if (e < 0.2f) return BlockType.WATER;
        else if (e < 0.35f) return BlockType.SAND;
        else if (e < 0.6f) return BlockType.GRASS;
        else if (e < 0.8f) return BlockType.STONE;
        else return BlockType.SNOW;
    }

    int Height(float e) {
        return Mathf.RoundToInt(e * DEPTH);
    }
}