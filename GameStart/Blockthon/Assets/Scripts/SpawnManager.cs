using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoSingleton<SpawnManager>, IGameObserver
{
    public Transform[] spawnPoints;
    public GameObject blockPrefabEasy;
    public GameObject blockPrefabHard;

    private float timeToSpawn = 2f;
    private bool pauseSpawn = false;
    private float forwardForce;
    private int currentLevel;
    float[] LEVELFORCES = { 3000f, 4000f, 6000f, 8000f };
    float[] SPAWNTIME = { 2f, 1.75f, 1.50f, 1f };

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.AddLevelObserver(this);
        StartCoroutine(SpawnCoroutine());
    }

    public void PauseSpawn(bool pause)
    {
        pauseSpawn = pause;
    }

    void SpawnBlocks()
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            switch (currentLevel)
            {
                case 1:
                    if (randomIndex == i)
                    {
                        GameObject o = Instantiate(blockPrefabEasy, spawnPoints[i].position, Quaternion.identity);
                        ObstacleMovement m = o.GetComponent<ObstacleMovement>();
                        SetObstacleSpeed(m);
                    }
                    break;
                case 2:
                    if (randomIndex != i)
                    {
                        GameObject o = Instantiate(blockPrefabEasy, spawnPoints[i].position, Quaternion.identity);
                        ObstacleMovement m = o.GetComponent<ObstacleMovement>();
                        SetObstacleSpeed(m);
                    }
                    break;
                case 3:
                    if (randomIndex != i)
                    {
                        GameObject o = Instantiate(blockPrefabHard, spawnPoints[i].position, Quaternion.identity);
                        ObstacleMovement m = o.GetComponent<ObstacleMovement>();
                        SetObstacleSpeed(m);
                        SetObstacleScale(m);
                    }
                    break;
                case 4:
                    {
                        GameObject o = Instantiate(blockPrefabHard, spawnPoints[i].position, Quaternion.identity);
                        ObstacleMovement m = o.GetComponent<ObstacleMovement>();
                        SetObstacleSpeed(m);
                    }
                    break;
            }
        }
    }

 
    private void SetObstacleSpeed(ObstacleMovement m)
    {
        m.SetSpeed(forwardForce);
    }

    private Vector3 GenerateRandScale()
    {
        return new Vector3();
    }

    private void SetObstacleScale(ObstacleMovement m)
    {
        Vector3 randScale = GenerateRandScale();
        m.SetScale(randScale);
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            SpawnBlocks();

            yield return new WaitForSeconds(timeToSpawn);
            while (pauseSpawn)
            {
                yield return null;
            }
        }
    }
    private void updateSpawnSettings()
    {
        forwardForce = LEVELFORCES[currentLevel - 1];
        timeToSpawn = SPAWNTIME[currentLevel - 1];
    }

    public void Notify()
    {
        currentLevel = GameManager.Instance.GetLevel();
        updateSpawnSettings();
    }
}
