using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
//using System.Numerics;

public class LevelSettings
{ 
    public float minForce { get; private set; }
    public float maxForce { get; private set; }
    public int minObstacleWidth { get; private set; }
    public int maxObstacleWidth { get; private set; }
    public int minObstacleHeight { get; private set; }
    public int maxObstacleHeight { get; private set; }
    public float offsetXMin { get; private set; }
    public float offsetXMax { get; private set; }
    public float offsetZMin { get; private set; }
    public float offsetZMax { get; private set; }
    public Color color { get; private set; }
    public LevelSettings(
        float minF, float maxF, 
        int minW, int maxW, 
        int minH, int maxH,  
        float minXO, float  maxXO, 
        float minZO, float maxZO, 
        Color c)
    {
        minForce = minF;
        maxForce = maxF;
        offsetXMin = minXO;
        offsetXMax = maxXO;
        offsetZMin = minZO;
        offsetZMax = maxZO;
        minObstacleWidth = minW;
        maxObstacleWidth = maxW;
        minObstacleHeight = minH;
        maxObstacleHeight = maxH;
        color = c;
    }
}

public class LevelSpecs
{
    public static float L1_MIN_F = 3000f;
    public static float L1_MAX_F = 3500f;
    public static int L1_MIN_W = 1;
    public static int L1_MAX_W = 3;
    public static int L1_MIN_H = 1;
    public static int L1_MAX_H = 1;
    public static float L1_MIN_XO = -1f;
    public static float L1_MAX_XO = 2f;
    public static float L1_MIN_ZO = -1f;
    public static float L1_MAX_ZO = 2f;
    public static Color L1_COLOR = new Color(18/255f, 103/255f, 154/255f);

    public static float L2_MIN_F = 4000f;
    public static float L2_MAX_F = 4500f;
    public static int L2_MIN_W = 2;
    public static int L2_MAX_W = 4;
    public static int L2_MIN_H = 1;
    public static int L2_MAX_H = 3;
    public static float L2_MIN_XO = -2f;
    public static float L2_MAX_XO = 3f;
    public static float L2_MIN_ZO = -2f;
    public static float L2_MAX_ZO = 3f;
    public static Color L2_COLOR = new Color(18/255f, 103/255f, 154/255f);

    public static float L3_MIN_F = 6000f;
    public static float L3_MAX_F = 6500f;
    public static int L3_MIN_W = 3;
    public static int L3_MAX_W = 6;
    public static int L3_MIN_H = 1;
    public static int L3_MAX_H = 3;
    public static float L3_MIN_XO = -2f;
    public static float L3_MAX_XO = 3f;
    public static float L3_MIN_ZO = -2f;
    public static float L3_MAX_ZO = 4f;
    public static Color L3_COLOR = new Color(253/255f, 140/255f, 0);

    public static float L4_MIN_F = 7500f;
    public static float L4_MAX_F = 8000f;
    public static int L4_MIN_W = 3;
    public static int L4_MAX_W = 6;
    public static int L4_MIN_H = 1;
    public static int L4_MAX_H = 3;
    public static float L4_MIN_XO = -2f;
    public static float L4_MAX_XO = 2f;
    public static float L4_MIN_ZO = -2f;
    public static float L4_MAX_ZO = 4f;
    public static Color L4_COLOR = new Color(253/255f, 140/255f, 0);

    private LevelSettings[] settings;
    public LevelSpecs()
    {
        settings = new LevelSettings[4];

        settings[0] = new LevelSettings(
            L1_MIN_F, L1_MAX_F, L1_MIN_W, L1_MAX_W, L1_MIN_H, L1_MAX_H, L1_MIN_XO, L1_MAX_XO, L1_MIN_ZO, L1_MAX_ZO, L1_COLOR);
        settings[1] = new LevelSettings(
            L2_MIN_F, L2_MAX_F, L2_MIN_W, L2_MAX_W, L2_MIN_H, L2_MAX_H, L2_MIN_XO, L2_MAX_XO, L2_MIN_ZO, L2_MAX_ZO, L2_COLOR);
        settings[2] = new LevelSettings(
            L3_MIN_F, L3_MAX_F, L3_MIN_W, L3_MAX_W, L3_MIN_H, L3_MAX_H, L3_MIN_XO, L3_MAX_XO, L3_MIN_ZO, L3_MAX_ZO, L3_COLOR);
        settings[3] = new LevelSettings(
            L4_MIN_F, L4_MAX_F, L4_MIN_W, L4_MAX_W, L4_MIN_H, L4_MAX_H, L4_MIN_XO, L4_MAX_XO, L4_MIN_ZO, L4_MAX_ZO, L4_COLOR);

    }
    public LevelSettings Get(int level)
    {
        return settings[level - 1];
    }
}

public class SpawnManager : MonoSingleton<SpawnManager>, IGameObserver
{
    public Transform[] spawnPoints;
    public ObstacleMovement blockPrefab;
    private LevelSpecs levelSpecs;
    private float timeToSpawn = 2f;
    private bool spawnEnabled = true;
    private int currentLevel;
    public static float PATH_LEFT = -7.5f;
    public static float PATH_RIGHT = 7.5f;

    float[] SPAWNTIME = { 2f, 1.75f, 1.50f, 1f };

    // Start is called before the first frame update
    void Start()
    {
        levelSpecs = new LevelSpecs();
        GameManager.Instance.AddLevelObserver(this);
        StartCoroutine(SpawnCoroutine());
    }

    public void SetSpawn(bool active)
    {
        spawnEnabled = active;
        ToggleObstacles(active);
    }

    void SpawnBlocks()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            switch (currentLevel)
            {
                case 1:
                    if (randomIndex == i)
                    {
                        ObstacleMovement m = Instantiate(blockPrefab, spawnPoints[i].position, Quaternion.identity);
                        SetObstacleFeatures(m, spawnPoints[i].position.x);
                    }
                    break;
                case 2:
                case 3:
                    if (randomIndex != i)
                    {
                        ObstacleMovement m = Instantiate(blockPrefab, spawnPoints[i].position, Quaternion.identity);
                        SetObstacleFeatures(m, spawnPoints[i].position.x);
                    }
                    break;
                case 4:
                    {
                        ObstacleMovement m = Instantiate(blockPrefab, spawnPoints[i].position, Quaternion.identity);
                        SetObstacleFeatures(m, spawnPoints[i].position.x);
                    }
                    break;
            }
        }
    }

    private void ToggleObstacles(bool active)
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject o in obstacles)
        {
            o.SetActive(active);
        }
    }

    private void SetObstacleFeatures(ObstacleMovement m, float currentXPosition)
    {
        LevelSettings s = levelSpecs.Get(currentLevel);

        // Speed
        float speed = Random.Range(s.minForce, s.maxForce);
        m.SetSpeed(speed);

        // Scale
        float width = Random.Range(s.minObstacleWidth, s.maxObstacleWidth);
        float height = Random.Range(s.minObstacleHeight, s.maxObstacleHeight);
        m.SetScale(new Vector3(width, height, 1f));

        // Position 
        float offsetX = Random.Range(s.offsetXMin, s.offsetXMax);
        float offsetZ = Random.Range(s.offsetZMin, s.offsetZMax);
        float halfWidth = width / 2;
        float leftMargin = Mathf.Min(0, PATH_LEFT - (currentXPosition - halfWidth));
        float rightMargin = Mathf.Max(0, PATH_RIGHT - (currentXPosition + halfWidth));
        offsetX = Mathf.Clamp(offsetX, leftMargin, rightMargin);
        Vector3 offset = new Vector3(offsetX, 0f, offsetZ);
        m.SetPositionOffset(offset);

        // Color
        Color color = s.color;
        Renderer r = m.GetComponent<Renderer>();
        r.material.SetColor("_Color", color);
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            SpawnBlocks();

            yield return new WaitForSeconds(timeToSpawn);
            while (!spawnEnabled)
            {
                yield return null;
            }
        }
    }
    private void updateSpawnSettings()
    {
        timeToSpawn = SPAWNTIME[currentLevel - 1];
    }

    public void Notify()
    {
        currentLevel = GameManager.Instance.GetLevel();
        updateSpawnSettings();
    }
}
