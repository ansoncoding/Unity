using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class GameManager : MonoSingleton<GameManager>
{
    public Transform[] spawnPoints;
    public GameObject blockPrefabEasy;
    public GameObject blockPrefabHard;
    public GameObject[] completeLevelPanels;
    public PlayerMovement playerMovement;

    private const int LEVEL1_COMPLETE = 50;
    private const int LEVEL2_COMPLETE = 200;
    private const int LEVEL3_COMPLETE = 500;
    private const int LEVEL4_COMPLETE = 1000;
    public const float PLAYER_POSNZ = -46f; 
    private const float PLAYER_POSNY = 1.5f;
    private const float PLAYER_POSNX = 0f;
    private Vector3 startPosition = new Vector3(PLAYER_POSNX, PLAYER_POSNY, PLAYER_POSNZ);

    private float timeToSpawn = 2f;

    private bool died = false;
    private float timeToWaitUser = 0.1f;

    private int totalPoints = 0;
    private bool PauseSpawn = false;
    private int currentLevel = 1;
    private int lives = 3;
    
    private List<IGameObserver> scoreObservers;
    private List<IGameObserver> healthObservers;
    private List<IGameObserver> levelObservers;
    public void AddHealthObserver(IGameObserver o)
    {
        healthObservers.Add(o);
    }
    public void RemoveHealthObserver(IGameObserver o)
    {
        healthObservers.Remove(o);
    }
    private void NotifyHealthObservers()
    {
        foreach (IGameObserver o in healthObservers)
        {
            o.Notify();
        }
    }
    public void AddLevelObserver(IGameObserver o)
    {
        levelObservers.Add(o);
    }
    public void RemoveLevelObserver(IGameObserver o)
    {
        levelObservers.Remove(o);
    }
    private void NotifyLevelObservers()
    {
        foreach (IGameObserver o in levelObservers)
        {
            o.Notify();
        }
    }
    public void AddScoreObserver(IGameObserver o)
    {
        scoreObservers.Add(o);
    }
    public void RemoveScoreObserver(IGameObserver o)
    {
        scoreObservers.Remove(o);
    }
    private void NotifyScoreObservers()
    {
        foreach(IGameObserver o in scoreObservers)
        {
            o.Notify();
        }
    }

    void StartLevelCoroutine()
    {
        StartCoroutine(LevelUp());
    }
    void LoadWin()
    {
        SceneManager.LoadScene("GameWon");
    }
    void LoadGameover()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void IncPoints()
    {
        totalPoints += currentLevel * 10;

        NotifyScoreObservers();

        if ((currentLevel == 1 && totalPoints >= LEVEL1_COMPLETE) ||
            (currentLevel == 2 && totalPoints >= LEVEL2_COMPLETE) ||
            (currentLevel == 3 && totalPoints >= LEVEL3_COMPLETE))
        {
            Invoke("StartLevelCoroutine", 0.5f);
        }
        else if (currentLevel == 4 && totalPoints >= LEVEL4_COMPLETE) {
            // Win
            Invoke("LoadWin", 0.5f);
        }

    }

    public void CheckHit()
    {
        GameObject[] obstacles;
        Vector3 playerPosition = playerMovement.rb.position;

        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        
        // Sort the obstacles, closest ones to the player first
        Array.Sort(obstacles, (a, b)=> a.transform.position.z.CompareTo(b.transform.position.z));

        foreach (GameObject o in obstacles)
        {
            if (o.transform.position.z > -45)
            {
                float left = o.transform.position.x + o.transform.localScale.x / 2;
                float right = o.transform.position.x - o.transform.localScale.x / 2;
                float up = o.transform.position.y + o.transform.localScale.y / 2;
                float down = o.transform.position.y - o.transform.localScale.y / 2;
                
                if (playerPosition.x < left && playerPosition.x > right &&
                    playerPosition.y < up && playerPosition.y > down)
                {
                    Destroy(o);
                    break;
                }
            }
        }
    }

    void DestroyObstacles()
    {
        PauseSpawn = false;
        GameObject[] obstacles;

        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject o in obstacles)
        {
            Destroy(o);
        }
    }

    IEnumerator LevelUp()
    {
        completeLevelPanels[currentLevel - 1].SetActive(true);
        DestroyObstacles();
        
        PauseSpawn = true;
        while (!Input.GetKey(KeyCode.Return))
        {
            yield return new WaitForSeconds(timeToWaitUser);
        }
        completeLevelPanels[currentLevel - 1].SetActive(false);
        currentLevel++;
        NotifyLevelObservers();
        PauseSpawn = false;
    }
    
    public int GetLevel()
    {
        return currentLevel;
    }

    public int GetLives()
    {
        return lives;
    }

    public int GetPoints()
    {
        return totalPoints;
    }
   
    public override void Init ()
    {
        if (healthObservers == null)
        {
            healthObservers = new List<IGameObserver>();
        }
        if (scoreObservers == null)
        {
            scoreObservers = new List<IGameObserver>();
        }
        if (levelObservers == null)
        {
            levelObservers = new List<IGameObserver>();
        }
    }

    void Revive()
    {
        playerMovement.rb.position = startPosition;
        died = false;
        playerMovement.enabled = true;
    }
    public void EndGame()
    {
        if (!died)
        {
            died = true;
            lives--;
            playerMovement.enabled = false;
            NotifyHealthObservers();

            if (lives == 0)
            {
                Invoke("LoadGameover", 0.5f);
            }
            else
            {
                Invoke("Revive", 0.5f);
            }
        }
    }

    void SpawnBlocks()
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            switch (currentLevel) {
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

    void SetObstacleSpeed(ObstacleMovement m)
    {
        try
        {
            m.SetSpeed(currentLevel);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            SpawnBlocks();
            
            yield return new WaitForSeconds(timeToSpawn);
            while (PauseSpawn)
            {
                yield return null;
            }
        }
    }

    void Start()
    {
        NotifyScoreObservers();
        NotifyHealthObservers();
        NotifyLevelObservers();
        StartCoroutine(SpawnCoroutine());
    }
}
