using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour, IGameObserver
{
    public Text score;
    public Text lives;
    public Text level;

    void Start()
    {
        GameManager.Instance.AddScoreObserver(this);
        GameManager.Instance.AddLevelObserver(this);
        GameManager.Instance.AddHealthObserver(this);
    }
    public void Notify()
    {
        score.text = GameManager.Instance.GetPoints().ToString();
        lives.text = "Lives: " + GameManager.Instance.GetLives().ToString();
        level.text = "Level: " + GameManager.Instance.GetLevel().ToString();
    }
}
