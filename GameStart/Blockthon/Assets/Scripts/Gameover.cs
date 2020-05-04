using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }
    public void Restart()
    {
        Debug.Log("RESTART");
        SceneManager.LoadScene("Levels");
    }
}
