using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Welcome : MonoBehaviour
{ 
    public void OnStart()
    {
        Debug.Log("RESTART");
        SceneManager.LoadScene("Levels");
    }
}
