using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void RestartRun()
    {
        //Restarts the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
